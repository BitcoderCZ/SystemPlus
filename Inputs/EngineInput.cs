﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SystemPlus.Extensions;
using SystemPlus.Vectors;

namespace Systemplus.Inputs
{
    internal class EngineInput :
        Input
    {
        /*#region storage

        /// <summary>
        /// Actual control which provides data.
        /// </summary>
        public System.Windows.Forms.Control Control { get; private set; }

        /// <inheritdoc />
        public override System.Drawing.Size Size => Control.Size;

        /// <inheritdoc />
        public override event SizeEventHandler SizeChanged;

        /// <inheritdoc />
        public override event MouseEventHandler MouseMove;

        /// <inheritdoc />
        public override event MouseEventHandler MouseDown;

        /// <inheritdoc />
        public override event MouseEventHandler MouseUp;

        /// <inheritdoc />
        public override event MouseEventHandler MouseWheel;

        /// <inheritdoc />
        public override event KeyEventHandler KeyDown;

        /// <inheritdoc />
        public override event KeyEventHandler KeyUp;

        #endregion

        #region ctor

        /// <inheritdoc />
        public EngineInput(System.Windows.Forms.Control control)
        {
            // store
            Control = control;

            // hook
            Control.SizeChanged += ControlOnSizeChanged;
            Control.MouseMove += ControlOnMouseMove;
            Control.MouseDown += ControlOnMouseDown;
            Control.MouseUp += ControlOnMouseUp;
            Control.MouseWheel += ControlOnMouseWheel;
            Control.KeyDown += ControlOnKeyDown;
            Control.KeyUp += ControlOnKeyUp;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            // unhook
            Control.SizeChanged -= ControlOnSizeChanged;
            Control.MouseMove -= ControlOnMouseMove;
            Control.MouseDown -= ControlOnMouseDown;
            Control.MouseUp -= ControlOnMouseUp;
            Control.MouseWheel -= ControlOnMouseWheel;
            Control.KeyDown -= ControlOnKeyDown;
            Control.KeyUp -= ControlOnKeyUp;

            // clear storage
            Control = default;
        }

        #endregion

        #region handlers

        /// <inheritdoc cref="SizeChanged" />
        private void ControlOnSizeChanged(object sender, System.EventArgs args) => SizeChanged?.Invoke(sender, new SizeEventArgs(Control.Size));

        /// <inheritdoc cref="MouseMove" />
        private void ControlOnMouseMove(object sender, System.Windows.Forms.MouseEventArgs args) => MouseMove?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="MouseDown" />
        private void ControlOnMouseDown(object sender, System.Windows.Forms.MouseEventArgs args) => MouseDown?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="MouseUp" />
        private void ControlOnMouseUp(object sender, System.Windows.Forms.MouseEventArgs args) => MouseUp?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="MouseWheel" />
        private void ControlOnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs args) => MouseWheel?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="KeyDown" />
        private void ControlOnKeyDown(object sender, System.Windows.Forms.KeyEventArgs args) => KeyDown?.Invoke(sender, new KeyEventArgs(args));

        /// <inheritdoc cref="KeyUp" />
        private void ControlOnKeyUp(object sender, System.Windows.Forms.KeyEventArgs args) => KeyUp?.Invoke(sender, new KeyEventArgs(args));

        #endregion*/


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        #region storage

        public System.Windows.Forms.Control Control { get; set; }

        public override System.Drawing.Size Size => new System.Drawing.Size((int)Control.Width, (int)Control.Height);

        public override event SizeEventHandler SizeChanged;

        public override event MouseEventHandler MouseMove;

        public override event MouseEventHandler MouseDown;

        public override event MouseEventHandler MouseUp;

        public override event MouseEventHandler MouseWheel;

        public override event KeyEventHandler KeyDown;

        public override event KeyEventHandler KeyUp;

        #endregion

        #region ctor

        public EngineInput(System.Windows.Forms.Control control)
        {
            Control = control;

            Control.SizeChanged += ControlOnSizeChanged;
            Control.MouseMove += ControlOnMouseMove;
            Control.MouseDown += ControlOnMouseDown;
            Control.MouseUp += ControlOnMouseUp;
            Control.MouseWheel += ControlOnMouseWheel;
            Control.KeyDown += ControlOnKeyDown;
            Control.KeyUp += ControlOnKeyUp;

            keys = new InputKey[173];
            for (int i = 0; i < 173; i++)
            {
                keysDown[(Key)i] = false;
                KeyModifiers[(Key)i] = Modifiers.None;
                keys[i] = new InputKey((Key)i, true);
            }

            Hook();
        }

        public override void Dispose()
        {
            UnHook();

            Control.SizeChanged -= ControlOnSizeChanged;
            Control.MouseMove -= ControlOnMouseMove;
            Control.MouseDown -= ControlOnMouseDown;
            Control.MouseUp -= ControlOnMouseUp;
            Control.MouseWheel -= ControlOnMouseWheel;
            Control.KeyDown -= ControlOnKeyDown;
            Control.KeyUp -= ControlOnKeyUp;

            Control = default;
        }

        #endregion

        #region handlers

        /// <inheritdoc cref="SizeChanged" />
        private void ControlOnSizeChanged(object sender, System.EventArgs args) => SizeChanged?.Invoke(sender, new SizeEventArgs(Control.Size));

        /// <inheritdoc cref="MouseMove" />
        private void ControlOnMouseMove(object sender, System.Windows.Forms.MouseEventArgs args) => MouseMove?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="MouseDown" />
        private void ControlOnMouseDown(object sender, System.Windows.Forms.MouseEventArgs args) => MouseDown?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="MouseUp" />
        private void ControlOnMouseUp(object sender, System.Windows.Forms.MouseEventArgs args) => MouseUp?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="MouseWheel" />
        private void ControlOnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs args) => MouseWheel?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="KeyDown" />
        private void ControlOnKeyDown(object sender, System.Windows.Forms.KeyEventArgs args) => KeyDown?.Invoke(sender, new KeyEventArgs(args));

        /// <inheritdoc cref="KeyUp" />
        private void ControlOnKeyUp(object sender, System.Windows.Forms.KeyEventArgs args) => KeyUp?.Invoke(sender, new KeyEventArgs(args));

        #endregion

        /*#region routines

        private Vector2D GetPosition(System.Windows.Input.MouseEventArgs args)
        {
            return args.GetPosition(Control).ToVector2D();
        }

        #endregion*/

        public InputKey[] keys;
        public Dictionary<Key, bool> keysDown { get; } = new Dictionary<Key, bool>();
        public Dictionary<Key, Modifiers> KeyModifiers { get; } = new Dictionary<Key, Modifiers>();
        public MouseButtons mouseButtons { get; private set; } = MouseButtons.None;
        public Vector2Int mousePosScreen { get; private set; } = new Vector2Int(0, 0);
        public Vector2D mousePosView
        {
            get =>
            new Vector2D((float)mousePosScreen.x / (float)Size.Width,
            (float)mousePosScreen.y / (float)Size.Height);
        }

        public void Hook()
        {
            KeyDown += KeyDownA;
            KeyUp += KeyUpA;
            MouseDown += MouseDownA;
            MouseUp += MouseUpA;
            MouseMove += MouseMoveA;
        }
        public void UnHook()
        {
            KeyDown -= KeyDownA;
            KeyUp -= KeyUpA;
            MouseDown -= MouseDownA;
            MouseUp -= MouseUpA;
            MouseMove -= MouseMoveA;
        }


        private void KeyDownA(object sender, IKeyEventArgs args)
        {
            Console.WriteLine("POG");
            keysDown[args.Key] = true;
            Modifiers m = args.Modifiers;
            if ((((ushort)GetKeyState(0x14)) & 0xffff) != 0)
                m |= Modifiers.CapsLock;
            if ((((ushort)GetKeyState(0x90)) & 0xffff) != 0)
                m |= Modifiers.NumbLock;
            KeyModifiers[args.Key] = m;
        }
        private void KeyUpA(object sender, IKeyEventArgs args)
        {
            keysDown[args.Key] = false;
            KeyModifiers[args.Key] = Modifiers.None;
        }
        private void MouseDownA(object sender, IMouseEventArgs args)
        {
            mouseButtons |= args.Buttons;
        }
        private void MouseUpA(object sender, IMouseEventArgs args)
        {
            mouseButtons &= (~args.Buttons);
        }
        private void MouseMoveA(object sender, IMouseEventArgs args)
        {
            mousePosScreen = args.Position.ToVector2Int();
        }


        public InputKey GetKey(Key key) => keys[(int)key];

        public void Update()
        {
            for (int i = 0; i < 173; i++)
            {
                keys[i].Update(keysDown[(Key)i]);
            }
        }
    }

    public struct InputKey
    {
        public readonly Key Key;

        public bool pressed { get; private set; }
        public bool pressedDown { get; private set; }
        public bool pressedUp { get; private set; }

        public int pressedDuration { get; private set; }

        public InputKey(Key key, bool hm)
        {
            Key = key;

            pressed = false;
            pressedDown = hm;
            pressedUp = hm;
            pressedDuration = 0;
        }

        public void Update(bool currentFrame)
        {
            if (pressed == false && currentFrame == true)
            {
                pressedDown = true;
                pressedUp = false;
                pressedDuration = 0;
            }
            else if (pressed == true && currentFrame == true)
            {
                pressedDown = false;
                pressedUp = false;
                pressedDuration++;
            }
            else if (pressed == true && currentFrame == false)
            {
                pressedDown = false;
                pressedUp = true;
                pressedDuration = 0;
            }
            else if (pressed == false && currentFrame == false)
            {
                pressedDown = false;
                pressedUp = false;
            }
            pressed = currentFrame;
        }

        public override string ToString()
        {
            return $"pr: {pressed}, down: {pressedDown}, up: {pressedUp}";
        }
    }
}
