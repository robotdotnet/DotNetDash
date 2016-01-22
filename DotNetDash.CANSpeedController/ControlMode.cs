namespace DotNetDash.CANSpeedController
{
    /// <summary>
    /// Current mode as reported by the CAN Speed Controller to NetworkTables.
    /// </summary>
    public enum ControlMode
    {
        /// <summary>
        /// Percent Vbus Mode (Similar to PWM).
        /// </summary>
        PercentVbus = 0,
        /// <summary>
        /// Follower Mode (sets the controller to follow another controller).
        /// </summary>
        Follower = 5,
        /// <summary>
        /// Runs the controller by directly setting the output voltage.
        /// </summary>
        Voltage = 4,
        /// <summary>
        /// Runs the controller in Closed Loop Position mode.
        /// </summary>
        Position = 1,
        /// <summary>
        /// Runs the controller in Closed Loop Speed mode.
        /// </summary>
        Speed = 2,
        /// <summary>
        /// Runs the controller in Closed Loop Current mode.
        /// </summary>
        Current = 3,

        /// <summary>
        /// Runs the controller in Motion Profile mode.
        /// </summary>
        MotionProfile = 6,

        /// <summary>
        /// If this mode is set, the controller is disabled.
        /// </summary>
        Disabled = 15
    }
}
