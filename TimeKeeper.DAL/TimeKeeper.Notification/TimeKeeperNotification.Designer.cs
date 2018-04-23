namespace TimeKeeper.Notification
{
    partial class TimeKeeperNotification
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timeDelay = new System.Windows.Forms.Timer(this.components);
            this.serviceController1 = new System.ServiceProcess.ServiceController();
            // 
            // timeDelay
            // 
            this.timeDelay.Interval = 60000;
            // 
            // TimeKeeperNotification
            // 
            this.ServiceName = "Service1";

        }

        #endregion

        private System.Windows.Forms.Timer timeDelay;
        private System.ServiceProcess.ServiceController serviceController1;
    }
}
