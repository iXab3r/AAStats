using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using XMLib;

namespace AAStats
{
    public class MoveableUserControl : UserControl
    {
        public MoveableUserControl()
        {
            this.ControlAdded += OnControlAdded;
            this.ControlRemoved += OnControlRemoved;
            SubscribeToEvents(this);
        }

        private void SubscribeToEvents(Control _ctrl)
        {
	        if (_ctrl == null)
	        {
		        throw new ArgumentNullException(nameof(_ctrl));
	        }

	        _ctrl.MouseDown += Control_MouseDown;
            _ctrl.MouseUp += Control_MouseUp;
            _ctrl.MouseMove += Control_MouseMove;
        }

        private void UnsubscribeFromEvents(Control _ctrl)
        {
	        if (_ctrl == null)
	        {
		        throw new ArgumentNullException(nameof(_ctrl));
	        }

            _ctrl.MouseDown -= Control_MouseDown;
            _ctrl.MouseUp -= Control_MouseUp;
            _ctrl.MouseMove -= Control_MouseMove;
        }

        private void OnControlAdded(object _sender, ControlEventArgs _controlEventArgs)
        {
            if (_controlEventArgs == null || _controlEventArgs.Control == null)
            {
                return;
            }
            var control = _controlEventArgs.Control;
            SubscribeToEvents(control);
        }

        private void OnControlRemoved(object _sender, ControlEventArgs _controlEventArgs)
        {
            if (_controlEventArgs == null || _controlEventArgs.Control == null)
            {
                return;
            }
            var control = _controlEventArgs.Control;
            UnsubscribeFromEvents(control);
        }

        public new event MouseEventHandler MouseDown
        {
            add
            {
                base.MouseDown += value;
            }
            remove
            {
                base.MouseDown -= value;
            }
        }

        private Point pointMouse = new Point();
        private Control ctrlMoved = new Control();
        private bool bMoving = false;
        private void Control_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if not left mouse button, exit
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            // save cursor location
            pointMouse = e.Location;
            //remember that we're moving
            bMoving = true;
            Debug.WriteLine("Start dragging ctrl {0} @ {1}", sender, pointMouse);
        }
        private void Control_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            bMoving = false;
            Debug.WriteLine("STOP dragging ctrl {0} @ {1}", sender, pointMouse);
        }
        private void Control_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if not being moved or left mouse button not used, exit
            if (!bMoving || e.Button != MouseButtons.Left)
            {
                return;
            }
            //get control reference
            ctrlMoved = (Control)sender;
            //set control's position based upon mouse's position change
            this.Left += e.X - pointMouse.X;
            this.Top += e.Y - pointMouse.Y;
        }
    }
}