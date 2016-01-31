using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using System.Threading;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;

namespace DotNetDash.CameraViews
{
    /// <summary>
    /// Connects to a roboRIO and reads the non-standard MJPEG stream that it supplies
    /// </summary>
    class FrcMJpegStream : IVideoSource
    {
        private readonly int teamNumber;
        private const int CameraServerPort = 1180;
        private const int HwCompressionId = -1;
        private const int Size640x480 = 0;
        private int framesPerSecond = 30;
        private Task videoProcessorTask;
        private CancellationTokenSource cancelTokenSource;
        private long bytesReceived;
        private int framesReceived;

        public FrcMJpegStream(int teamNumber)
        {
            this.teamNumber = teamNumber;
        }

        public event NewFrameEventHandler NewFrame;
        public event PlayingFinishedEventHandler PlayingFinished;
        public event VideoSourceErrorEventHandler VideoSourceError;

        public long BytesReceived
        {
            get
            {
                return bytesReceived;
            }
        }

        public int FramesReceived
        {
            get
            {
                return framesReceived;
            }
        }

        public bool IsRunning
        {
            get
            {
                return videoProcessorTask != null;
            }
        }

        public string Source
        {
            get
            {
                return $"RoboRIO for team: {teamNumber}";
            }
        }

        public void SignalToStop()
        {
            cancelTokenSource.Cancel();
            videoProcessorTask = null;
        }

        public void Start()
        {
            if (videoProcessorTask == null)
            {

                cancelTokenSource = new CancellationTokenSource();
                var token = cancelTokenSource.Token;
                videoProcessorTask = ProcessVideoAsync(token); 
            }
        }

        public void Stop()
        {
            cancelTokenSource.Cancel();
            videoProcessorTask = null;
        }

        private async Task ProcessVideoAsync(CancellationToken token)
        {
            await Task.Yield();
            try
            {
                using (Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    await Task.Factory.FromAsync(clientSocket.BeginConnect, clientSocket.EndConnect,
                        $"roborio-{teamNumber}-FRC.local", CameraServerPort, null);
                    using (var socketStream = new NetworkStream(clientSocket))
                    {
                        await socketStream.WriteAsync(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(framesPerSecond)), 0, 4, token);
                        bytesReceived += 4;
                        await socketStream.WriteAsync(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(HwCompressionId)), 0, 4, token);
                        bytesReceived += 4;
                        await socketStream.WriteAsync(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Size640x480)), 0, 4, token);
                        bytesReceived += 4;
                        while (true)
                        {
                            token.ThrowIfCancellationRequested();
                            var magicToken = new byte[4];
                            await socketStream.ReadAsync(magicToken, 0, 4, token);
                            bytesReceived += 4;
                            if (BitConverter.ToInt32(magicToken, 0) != 0x1000)
                            {
                                //Magic token did not match
                                return;
                            }
                            var imageLengthBytes = new byte[4];
                            await socketStream.ReadAsync(imageLengthBytes, 0, 4, token);
                            bytesReceived += 4;
                            using (var frame = new System.Drawing.Bitmap(socketStream))
                            {
                                NewFrame?.Invoke(this, new NewFrameEventArgs(frame)); 
                            }
                            bytesReceived += IPAddress.NetworkToHostOrder(BitConverter.ToInt32(imageLengthBytes, 0));
                            framesReceived++;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                PlayingFinished?.Invoke(this, ReasonToFinishPlaying.StoppedByUser);
            }
            catch (Exception ex)
            {
                PlayingFinished?.Invoke(this, ReasonToFinishPlaying.VideoSourceError);
                VideoSourceError?.Invoke(this, new VideoSourceErrorEventArgs(ex.Message));
            }
        }

        public void WaitForStop()
        {
            cancelTokenSource.Cancel();
            videoProcessorTask = null;
        }
    }
}
