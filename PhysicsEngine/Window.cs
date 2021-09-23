/*
using PhysicsEngine.ObjectClasses;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhysicsEngine
{
    class Window : Form
    {
        public void Run()
        {
            // I
            #region Interface

            Panel Interface = new Panel();

            Panel GInterface = new Panel();

            int DistanceY = 10;

            #region Moveing Interface

            DragAndDrop PositionInterface = new DragAndDrop(Interface);

            bool ISetDefualtLocation = false;

            bool GISetDefualtLocation = false;

            bool TISetDefualtLocation = true;
            bool GTISetDefualtLocation = true;

            int gap = 100;

            Interface.MouseDoubleClick += ResetILocation;
            Interface.MouseDown += IMDown;
            Interface.MouseMove += IMMove;
            Interface.MouseUp += IMUp;

            void ResetILocation(object sender, MouseEventArgs mouse)
            {
                Interface.BringToFront();
                ISetDefualtLocation = true;
                TISetDefualtLocation = true;
                if (!GISetDefualtLocation)
                {
                    Interface.Location = new Point((Width / 2) - (Interface.Width / 2), DistanceY);
                }
                else
                {
                    Interface.Location = new Point((Width / 2) - (Interface.Width + (gap / 2)), DistanceY);
                    GInterface.Location = new Point((Width / 2) + (gap / 2), DistanceY);
                }
            }

            void IMDown(object sender, MouseEventArgs mouse)
            {
                Interface.BringToFront();
                PositionInterface.StartDragAndDrop();
                Core.Graphics.Focus();
            }

            void IMMove(object sender, MouseEventArgs mouse)
            {
                PositionInterface.ApplyDragAndDrop(0, 0, Width, Height, Interface.Width, Interface.Height);
                if (PositionInterface.IsDragAndDrop)
                {
                    ISetDefualtLocation = false;
                    TISetDefualtLocation = false;
                    if (GISetDefualtLocation)
                    {
                        GInterface.Location = new Point((Width / 2) - (GInterface.Width / 2), DistanceY);
                    }
                }
            }

            void IMUp(object sender, MouseEventArgs mouse)
            {
                PositionInterface.EndDragAndDrop();
                Core.Graphics.Focus();
            }


            DragAndDrop GPositionInterface = new DragAndDrop(GInterface);

            GInterface.MouseDoubleClick += GResetILocation;
            GInterface.MouseDown += GIMDown;
            GInterface.MouseMove += GIMMove;
            GInterface.MouseUp += GIMUp;

            void GResetILocation(object sender, MouseEventArgs mouse)
            {
                GInterface.BringToFront();
                GISetDefualtLocation = true;
                GTISetDefualtLocation = true;
                if (!ISetDefualtLocation)
                {
                    GInterface.Location = new Point((Width / 2) - (GInterface.Width / 2), DistanceY);
                }
                else
                {
                    Interface.Location = new Point((Width / 2) - (Interface.Width + (gap / 2)), DistanceY);
                    GInterface.Location = new Point((Width / 2) + (gap / 2), DistanceY);
                }
            }

            void GIMDown(object sender, MouseEventArgs mouse)
            {
                GInterface.BringToFront();
                GPositionInterface.StartDragAndDrop();
                Core.Graphics.Focus();
            }

            void GIMMove(object sender, MouseEventArgs mouse)
            {
                GPositionInterface.ApplyDragAndDrop(0, 0, Width, Height, GInterface.Width, GInterface.Height);
                if (GPositionInterface.IsDragAndDrop)
                {
                    GISetDefualtLocation = false;
                    GTISetDefualtLocation = false;
                    if (ISetDefualtLocation)
                    {
                        Interface.Location = new Point((Width / 2) - (Interface.Width / 2), DistanceY);
                    }
                }
            }

            void GIMUp(object sender, MouseEventArgs mouse)
            {
                GPositionInterface.EndDragAndDrop();
                Core.Graphics.Focus();
            }

            #endregion

            void LimitKeysD(object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }

                // only allow one negative sign
                if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
                {
                    e.Handled = true;
                }
            }

            void LimitKeysI(object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

            void LimitKeysR(object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != 'r'))
                {
                    e.Handled = true;
                }

                // only allow one r
                if ((e.KeyChar == 'r') && ((sender as TextBox).Text.IndexOf('r') > -1))
                {
                    e.Handled = true;
                }
            }

            #region Header

            Label Head = new Label();

            Interface.Controls.Add(Head);
            Head.Text = "Game Settings";
            Head.Location = new Point(10, 10);
            Head.Font = new Font("Serif", 15, FontStyle.Bold);
            Head.Size = new Size(Interface.Width - 20, 35);
            Head.TextAlign = ContentAlignment.MiddleCenter;
            Head.Size = new Size(0, 35);
            Head.MouseDoubleClick += ResetILocation;
            Head.MouseDown += IMDown;
            Head.MouseMove += IMMove;
            Head.MouseUp += IMUp;

            #endregion

            #region Gravity Set

            Label LGravity = new Label();

            Interface.Controls.Add(LGravity);
            LGravity.Text = "Gravity:";
            LGravity.Location = new Point(10, Head.Location.Y + Head.Height + 10);
            LGravity.Font = new Font("Serif", 11, FontStyle.Bold);
            LGravity.AutoSize = true;
            LGravity.MouseDoubleClick += ResetILocation;
            LGravity.MouseDown += IMDown;
            LGravity.MouseMove += IMMove;
            LGravity.MouseUp += IMUp;

            TextBox TGravity = new TextBox();

            Interface.Controls.Add(TGravity);
            TGravity.Location = new Point(LGravity.Width + 20, LGravity.Location.Y);
            TGravity.Size = new Size(50, LGravity.Height);
            TGravity.Text = Core.Gravity.ToString();
            TGravity.Font = new Font("Serif", 10, FontStyle.Regular);
            TGravity.TextChanged += SetGravity;
            TGravity.KeyPress += LimitKeysD;

            void SetGravity(object sender, EventArgs e)
            {
                try
                {
                    Core.Gravity = double.Parse(TGravity.Text);
                }
                catch (Exception)
                {
                    
                }
            }

            #endregion

            #region Speed Set

            Label LSpeed = new Label();

            Interface.Controls.Add(LSpeed);
            LSpeed.Text = "Speed:";
            LSpeed.Location = new Point(TGravity.Location.X + TGravity.Width + 40, LGravity.Location.Y);
            LSpeed.Font = new Font("Serif", 11, FontStyle.Bold);
            LSpeed.AutoSize = true;
            LSpeed.MouseDoubleClick += ResetILocation;
            LSpeed.MouseDown += IMDown;
            LSpeed.MouseMove += IMMove;
            LSpeed.MouseUp += IMUp;

            TextBox TSpeed = new TextBox();

            Interface.Controls.Add(TSpeed);
            TSpeed.Location = new Point(LSpeed.Location.X + LSpeed.Width + 10, LGravity.Location.Y);
            TSpeed.Size = new Size(50, LGravity.Height);
            TSpeed.Text = Core.Speed.ToString();
            TSpeed.Font = new Font("Serif", 10, FontStyle.Regular);
            TSpeed.TextChanged += SetSpeed;
            TSpeed.KeyPress += LimitKeysI;

            void SetSpeed(object sender, EventArgs e)
            {
                try
                {
                    Core.Speed = int.Parse(TSpeed.Text);
                    speed = Core.Speed;
                }
                catch (Exception)
                {

                }
            }

            #endregion

            #region Player Width Set

            Label LPlayerWidth = new Label();

            Interface.Controls.Add(LPlayerWidth);
            LPlayerWidth.Text = "Player Width:";
            LPlayerWidth.Location = new Point(10, LGravity.Height + LGravity.Location.Y + 10);
            LPlayerWidth.Font = new Font("Serif", 11, FontStyle.Bold);
            LPlayerWidth.AutoSize = true;
            LPlayerWidth.MouseDoubleClick += ResetILocation;
            LPlayerWidth.MouseDown += IMDown;
            LPlayerWidth.MouseMove += IMMove;
            LPlayerWidth.MouseUp += IMUp;

            TextBox TPlayerWidth = new TextBox();

            Interface.Controls.Add(TPlayerWidth);
            TPlayerWidth.Location = new Point(LPlayerWidth.Width + 20, LPlayerWidth.Location.Y);
            TPlayerWidth.Size = new Size(50, LGravity.Height);
            TPlayerWidth.Text = Player.Display.DisplaySize.Width.ToString();
            TPlayerWidth.Font = new Font("Serif", 10, FontStyle.Regular);
            TPlayerWidth.TextChanged += SetPlayerWidth;
            TPlayerWidth.KeyPress += LimitKeysI;

            void SetPlayerWidth(object sender, EventArgs e)
            {
                try
                {
                    Player.Width = int.Parse(TPlayerWidth.Text);

                    Player.SetDefaults(new DefaultProperty(
                        new GRectangle(
                            Player.Default.X, 
                            Player.Default.Y, 
                            int.Parse(TPlayerWidth.Text), 
                            Player.Default.Height), 
                        Player.Default.Display, 
                        Player.Default.Properties)
                    );

                    Moo.Width = int.Parse(TPlayerWidth.Text);

                    Moo.SetDefaults(new DefaultProperty(
                        new GRectangle(
                            Moo.Default.X,
                            Moo.Default.Y,
                            int.Parse(TPlayerWidth.Text),
                            Moo.Default.Height),
                        Moo.Default.Display,
                        Moo.Default.Properties)
                    );
                }
                catch (Exception)
                {
                    
                }
            }

            #endregion

            #region Scale Set

            Label LScale = new Label();

            Interface.Controls.Add(LScale);
            LScale.Text = "Scale:";
            LScale.Location = new Point(TPlayerWidth.Location.X + TPlayerWidth.Width + 7, TPlayerWidth.Location.Y);
            LScale.Font = new Font("Serif", 11, FontStyle.Bold);
            LScale.AutoSize = true;
            LScale.MouseDoubleClick += ResetILocation;
            LScale.MouseDown += IMDown;
            LScale.MouseMove += IMMove;
            LScale.MouseUp += IMUp;

            TextBox TScale = new TextBox();

            Interface.Controls.Add(TScale);
            TScale.Location = new Point(LScale.Location.X + LScale.Width + 10, TPlayerWidth.Location.Y);
            TScale.Size = new Size(50, LGravity.Height);
            TScale.Text = Core.Scale.ToString();
            TScale.Font = new Font("Serif", 10, FontStyle.Regular);
            TScale.TextChanged += SetScale;
            TScale.KeyPress += LimitKeysD;

            void SetScale(object sender, EventArgs e)
            {
                try
                {
                    Core.Scale = double.Parse(TScale.Text);
                }
                catch (Exception)
                {

                }
            }

            #endregion

            #region Player Height Set

            Label LPlayerHeight = new Label();

            Interface.Controls.Add(LPlayerHeight);
            LPlayerHeight.Text = "Player Height:";
            LPlayerHeight.Location = new Point(10, LPlayerWidth.Location.Y + LPlayerWidth.Height + 10);
            LPlayerHeight.Font = new Font("Serif", 11, FontStyle.Bold);
            LPlayerHeight.AutoSize = true;
            LPlayerHeight.MouseDoubleClick += ResetILocation;
            LPlayerHeight.MouseDown += IMDown;
            LPlayerHeight.MouseMove += IMMove;
            LPlayerHeight.MouseUp += IMUp;

            TextBox TPlayerHeight = new TextBox();

            Interface.Controls.Add(TPlayerHeight);
            TPlayerHeight.Location = new Point(LPlayerHeight.Width + 20, LPlayerHeight.Location.Y);
            TPlayerHeight.Size = new Size(50, LGravity.Height);
            TPlayerHeight.Text = Player.Display.DisplaySize.Height.ToString();
            TPlayerHeight.Font = new Font("Serif", 10, FontStyle.Regular);
            TPlayerHeight.TextChanged += SetPlayerHeight;
            TPlayerHeight.KeyPress += LimitKeysI;

            void SetPlayerHeight(object sender, EventArgs e)
            {
                try
                {
                    Player.Height = int.Parse(TPlayerHeight.Text);

                    Player.SetDefaults(new DefaultProperty(
                        new GRectangle(
                            Player.Default.X,
                            Player.Default.Y,
                            Player.Default.Width,
                            int.Parse(TPlayerHeight.Text)),
                        Player.Default.Display,
                        Player.Default.Properties)
                    );

                    Moo.Height = int.Parse(TPlayerHeight.Text);

                    Moo.SetDefaults(new DefaultProperty(
                        new GRectangle(
                            Moo.Default.X,
                            Moo.Default.Y,
                            Moo.Default.Width,
                            int.Parse(TPlayerHeight.Text)),
                        Moo.Default.Display,
                        Moo.Default.Properties)
                    );
                }
                catch (Exception)
                {

                }
            }

            #endregion

            #region Player Max Speed Set

            Label LPlayerMS = new Label();

            Interface.Controls.Add(LPlayerMS);
            LPlayerMS.Text = "Max Player Speed:";
            LPlayerMS.Location = new Point(10, LPlayerHeight.Location.Y + LPlayerHeight.Height + 10);
            LPlayerMS.Font = new Font("Serif", 11, FontStyle.Bold);
            LPlayerMS.AutoSize = true;
            LPlayerMS.MouseDoubleClick += ResetILocation;
            LPlayerMS.MouseDown += IMDown;
            LPlayerMS.MouseMove += IMMove;
            LPlayerMS.MouseUp += IMUp;

            TextBox TPlayerMS = new TextBox();

            Interface.Controls.Add(TPlayerMS);
            TPlayerMS.Location = new Point(LPlayerMS.Width + 20, LPlayerMS.Location.Y);
            TPlayerMS.Size = new Size(50, LGravity.Height);
            TPlayerMS.Text = playerMaxSpeed.ToString();
            TPlayerMS.Font = new Font("Serif", 10, FontStyle.Regular);
            TPlayerMS.TextChanged += SetPlayerMS;
            TPlayerMS.KeyPress += LimitKeysD;

            void SetPlayerMS(object sender, EventArgs e)
            {
                try
                {
                    playerMaxSpeed = double.Parse(TPlayerMS.Text);
                }
                catch (Exception)
                {

                }
            }

            #endregion

            #region Player Speed Increase Set

            Label LPlayerSI = new Label();

            Interface.Controls.Add(LPlayerSI);
            LPlayerSI.Text = "Player Speed Increase:";
            LPlayerSI.Location = new Point(10, LPlayerMS.Location.Y + LPlayerMS.Height + 10);
            LPlayerSI.Font = new Font("Serif", 11, FontStyle.Bold);
            LPlayerSI.AutoSize = true;
            LPlayerSI.MouseDoubleClick += ResetILocation;
            LPlayerSI.MouseDown += IMDown;
            LPlayerSI.MouseMove += IMMove;
            LPlayerSI.MouseUp += IMUp;

            TextBox TPlayerSI = new TextBox();

            Interface.Controls.Add(TPlayerSI);
            TPlayerSI.Location = new Point(LPlayerSI.Width + 20, LPlayerSI.Location.Y);
            TPlayerSI.Size = new Size(50, LGravity.Height);
            TPlayerSI.Text = playerVelocityIncrease.ToString();
            TPlayerSI.Font = new Font("Serif", 10, FontStyle.Regular);
            TPlayerSI.TextChanged += SetPlayerSI;
            TPlayerSI.KeyPress += LimitKeysD;

            void SetPlayerSI(object sender, EventArgs e)
            {
                try
                {
                    playerVelocityIncrease = double.Parse(TPlayerSI.Text);
                }
                catch (Exception)
                {

                }
            }

            #endregion

            #region Player Speed Change Set

            Label LPlayerSC = new Label();

            Interface.Controls.Add(LPlayerSC);
            LPlayerSC.Text = "Another Player Speed Varable:";
            LPlayerSC.Location = new Point(10, LPlayerSI.Location.Y + LPlayerSI.Height + 10);
            LPlayerSC.Font = new Font("Serif", 11, FontStyle.Bold);
            LPlayerSC.AutoSize = true;
            LPlayerSC.MouseDoubleClick += ResetILocation;
            LPlayerSC.MouseDown += IMDown;
            LPlayerSC.MouseMove += IMMove;
            LPlayerSC.MouseUp += IMUp;

            TextBox TPlayerSC = new TextBox();

            Interface.Controls.Add(TPlayerSC);
            TPlayerSC.Location = new Point(LPlayerSC.Width + 20, LPlayerSC.Location.Y);
            TPlayerSC.Size = new Size(50, LGravity.Height);
            TPlayerSC.Text = playerSpeedChange.ToString();
            TPlayerSC.Font = new Font("Serif", 10, FontStyle.Regular);
            TPlayerSC.TextChanged += SetPlayerSC;
            TPlayerSC.KeyPress += LimitKeysD;

            void SetPlayerSC(object sender, EventArgs e)
            {
                try
                {
                    playerSpeedChange = double.Parse(TPlayerSC.Text);
                }
                catch (Exception)
                {

                }
            }

            #endregion

            #region Header

            Label GHead = new Label();

            GInterface.Controls.Add(GHead);
            GHead.Text = "World Generation";
            GHead.Location = new Point(10, 10);
            GHead.Font = new Font("Serif", 15, FontStyle.Bold);
            GHead.TextAlign = ContentAlignment.MiddleCenter;
            GHead.Size = new Size(0, 35);
            GHead.MouseDoubleClick += GResetILocation;
            GHead.MouseDown += GIMDown;
            GHead.MouseMove += GIMMove;
            GHead.MouseUp += GIMUp;

            #endregion

            #region World Size Set

            Label LWorldSize = new Label();

            GInterface.Controls.Add(LWorldSize);
            LWorldSize.Text = "World Size:";
            LWorldSize.Location = new Point(10, GHead.Location.Y + GHead.Height + 10);
            LWorldSize.Font = new Font("Serif", 11, FontStyle.Bold);
            LWorldSize.AutoSize = true;
            LWorldSize.MouseDoubleClick += GResetILocation;
            LWorldSize.MouseDown += GIMDown;
            LWorldSize.MouseMove += GIMMove;
            LWorldSize.MouseUp += GIMUp;

            TextBox TWorldSize = new TextBox();

            GInterface.Controls.Add(TWorldSize);
            TWorldSize.Location = new Point(LWorldSize.Width + 20, LWorldSize.Location.Y);
            TWorldSize.Size = new Size(50, LGravity.Height);
            TWorldSize.Text = PosRange.ToString();
            TWorldSize.Font = new Font("Serif", 10, FontStyle.Regular);
            TWorldSize.KeyPress += LimitKeysI;

            Button BWorldSize = new Button();

            GInterface.Controls.Add(BWorldSize);
            BWorldSize.Location = new Point(TWorldSize.Location.X + TWorldSize.Width + 10, TWorldSize.Location.Y);
            BWorldSize.Size = new Size(50, LGravity.Height);
            BWorldSize.Text = "Set";
            BWorldSize.Font = new Font("Serif", 10, FontStyle.Regular);
            BWorldSize.Click += SetWorldSize;

            void SetWorldSize(object sender, EventArgs e)
            {
                try
                {
                    PosRange = int.Parse(TWorldSize.Text);

                    Core.GameBoundaryL = -(PosRange / 2);
                    Core.GameBoundaryR = PosRange / 2;
                    Core.GameBoundaryT = -(PosRange / 2);
                    Core.GameBoundaryB = PosRange / 2;
                }
                catch (Exception)
                {

                }
                Core.Graphics.Focus();
            }

            #endregion

            #region Num of Objects Set

            Label LRWorldN = new Label();

            GInterface.Controls.Add(LRWorldN);
            LRWorldN.Location = new Point(10, TWorldSize.Location.Y + TWorldSize.Height + 10);
            LRWorldN.Text = "Number of Objects:";
            LRWorldN.Font = new Font("Serif", 11, FontStyle.Bold);
            LRWorldN.AutoSize = true;
            LRWorldN.MouseDoubleClick += GResetILocation;
            LRWorldN.MouseDown += GIMDown;
            LRWorldN.MouseMove += GIMMove;
            LRWorldN.MouseUp += GIMUp;

            TextBox TRWorldN = new TextBox();

            GInterface.Controls.Add(TRWorldN);
            TRWorldN.Location = new Point(LRWorldN.Width + 20, LRWorldN.Location.Y);
            TRWorldN.Size = new Size(50, LGravity.Height);
            TRWorldN.Text = ObjectNumber.ToString();
            TRWorldN.Font = new Font("Serif", 10, FontStyle.Regular);
            TRWorldN.TextChanged += SetWorldN;
            TRWorldN.KeyPress += LimitKeysI;

            void SetWorldN(object sender, EventArgs e)
            {
                try
                {
                    ObjectNumber = int.Parse(TRWorldN.Text);
                }
                catch (Exception)
                {

                }
            }
            #endregion

            #region Max Width Set

            Label LMWidth = new Label();

            GInterface.Controls.Add(LMWidth);
            LMWidth.Location = new Point(10, TRWorldN.Location.Y + TRWorldN.Height + 10);
            LMWidth.Text = "Object Max Width:";
            LMWidth.Font = new Font("Serif", 11, FontStyle.Bold);
            LMWidth.AutoSize = true;
            LMWidth.MouseDoubleClick += GResetILocation;
            LMWidth.MouseDown += GIMDown;
            LMWidth.MouseMove += GIMMove;
            LMWidth.MouseUp += GIMUp;

            TextBox TMWidth = new TextBox();

            GInterface.Controls.Add(TMWidth);
            TMWidth.Location = new Point(LMWidth.Width + 20, LMWidth.Location.Y);
            TMWidth.Size = new Size(50, LGravity.Height);
            TMWidth.Text = WSizeRange.ToString();
            TMWidth.Font = new Font("Serif", 10, FontStyle.Regular);
            TMWidth.TextChanged += SetMWidth;
            TMWidth.KeyPress += LimitKeysI;

            void SetMWidth(object sender, EventArgs e)
            {
                try
                {
                    WSizeRange = int.Parse(TMWidth.Text);
                }
                catch (Exception)
                {

                }
            }
            #endregion

            #region Max Height Set

            Label LMHeight = new Label();

            GInterface.Controls.Add(LMHeight);
            LMHeight.Location = new Point(10, TMWidth.Location.Y + TMWidth.Height + 10);
            LMHeight.Text = "Object Max Height:";
            LMHeight.Font = new Font("Serif", 11, FontStyle.Bold);
            LMHeight.AutoSize = true;
            LMHeight.MouseDoubleClick += GResetILocation;
            LMHeight.MouseDown += GIMDown;
            LMHeight.MouseMove += GIMMove;
            LMHeight.MouseUp += GIMUp;

            TextBox TMHeight = new TextBox();

            GInterface.Controls.Add(TMHeight);
            TMHeight.Location = new Point(LMHeight.Width + 20, LMHeight.Location.Y);
            TMHeight.Size = new Size(50, LGravity.Height);
            TMHeight.Text = HSizeRange.ToString();
            TMHeight.Font = new Font("Serif", 10, FontStyle.Regular);
            TMHeight.TextChanged += SetMHeight;
            TMHeight.KeyPress += LimitKeysI;

            void SetMHeight(object sender, EventArgs e)
            {
                try
                {
                    HSizeRange = int.Parse(TMHeight.Text);
                }
                catch (Exception)
                {

                }
            }
            #endregion

            #region Generation Seed Set

            Label LGSeed = new Label();

            GInterface.Controls.Add(LGSeed);
            LGSeed.Location = new Point(10, TMHeight.Location.Y + TMHeight.Height + 10);
            LGSeed.Text = "Seed:";
            LGSeed.Font = new Font("Serif", 11, FontStyle.Bold);
            LGSeed.AutoSize = true;
            LGSeed.MouseDoubleClick += GResetILocation;
            LGSeed.MouseDown += GIMDown;
            LGSeed.MouseMove += GIMMove;
            LGSeed.MouseUp += GIMUp;

            TextBox TGSeed = new TextBox();

            GInterface.Controls.Add(TGSeed);
            TGSeed.Location = new Point(LGSeed.Width + 20, LGSeed.Location.Y);
            TGSeed.Size = new Size(50, LGravity.Height);
            TGSeed.Text = GenerationSeed.ToString();
            TGSeed.Font = new Font("Serif", 10, FontStyle.Regular);
            TGSeed.TextChanged += SetGSeed;
            TGSeed.KeyPress += LimitKeysR;

            void SetGSeed(object sender, EventArgs e)
            {
                try
                {
                    if (TGSeed.Text.Contains("r"))
                    {
                        IsRandomSeed = true;
                    }
                    else
                    {
                        IsRandomSeed = false;
                        GenerationSeed = int.Parse(TGSeed.Text);
                    }
                }
                catch (Exception)
                {

                }
            }
            #endregion

            #region Reload world

            Button BRWorld = new Button();

            GInterface.Controls.Add(BRWorld);
            BRWorld.Location = new Point(TGSeed.Width + TGSeed.Location.X + 10, LGSeed.Location.Y);
            BRWorld.Text = "Generate World!";
            BRWorld.Font = new Font("Serif", 10, FontStyle.Regular);
            BRWorld.AutoSize = true;
            BRWorld.Click += SetWorldSize;
            BRWorld.Click += RWorldSet;

            void RWorldSet(object sender, EventArgs e)
            {
                Platforms.ForEach(p => Core.RemoveStaticObject(ref p));

                Platforms.Clear();

                GenerateWorld();

                Core.Restart();

                Core.Graphics.Focus();
            }

            #endregion

            // Size accordingly

            #region Setting Interface

            Controls.Add(Interface);
            Interface.Size = new Size(TPlayerSC.Width + TPlayerSC.Location.X + 15, TPlayerSC.Height + TPlayerSC.Location.Y + 15);
            Interface.Location = new Point((Width / 2) - (Interface.Width + (gap / 2)), DistanceY);
            Interface.BackColor = Color.FromArgb(240, 240, 240);
            Interface.BorderStyle = BorderStyle.FixedSingle;
            Interface.Hide();
            Head.Size = new Size(Interface.Width - 20, 35);

            Controls.Add(GInterface);
            GInterface.Size = new Size(Interface.Width, Interface.Height);
            GInterface.Location = new Point((Width / 2) + (gap / 2), DistanceY);
            GInterface.BackColor = Color.FromArgb(240, 240, 240);
            GInterface.BorderStyle = BorderStyle.FixedSingle;
            GInterface.Hide();
            GHead.Size = new Size(GInterface.Width - 20, 35);

            #endregion

            #endregion

            // E
            #region Events

            bool CursorVis = true;

            void OnKeyDown(object sender, KeyEventArgs e)
            {
                else if (e.KeyCode == Keys.Tab)
                {
                    if (FormBorderStyle == FormBorderStyle.None)
                    {
                        FormBorderStyle = FormBorderStyle.Sizable;
                        WindowState = FormWindowState.Normal;
                    }
                    else
                    {
                        FormBorderStyle = FormBorderStyle.None;
                        WindowState = FormWindowState.Maximized;
                    }
                }
                else if (e.KeyCode == Keys.T)
                {
                    if (DisplayCon.Visible)
                    {
                        DisplayCon.Hide();
                    }
                    else
                    {
                        DisplayCon.Show();
                    }
                }
                else if (e.KeyCode == Keys.P)
                {
                    if (Interface.Visible)
                    {
                        ISetDefualtLocation = false;
                        Interface.Hide();
                        if (GISetDefualtLocation)
                        {
                            GInterface.Location = new Point((Width / 2) - (GInterface.Width / 2), DistanceY);
                        }
                        Core.Graphics.Focus();
                        if (!CursorVis && !GInterface.Visible)
                        {
                            Cursor.Hide();
                        }
                    }
                    else
                    {
                        ISetDefualtLocation = TISetDefualtLocation;
                        Interface.Show();
                        Interface.BringToFront();
                        Core.Graphics.Focus();
                        if (GISetDefualtLocation && ISetDefualtLocation)
                        {
                            GInterface.Location = new Point((Width / 2) + (gap / 2), DistanceY);
                            Interface.Location = new Point((Width / 2) - (Interface.Width + (gap / 2)), DistanceY);
                        }
                        else if (ISetDefualtLocation)
                        {
                            Interface.Location = new Point((Width / 2) - (Interface.Width / 2), DistanceY);
                        }
                        if (!CursorVis && !GInterface.Visible)
                        {
                            Cursor.Show();
                        }
                    }
                }
                else if (e.KeyCode == Keys.G)
                {
                    if (GInterface.Visible)
                    {
                        GISetDefualtLocation = false;
                        GInterface.Hide();
                        if (ISetDefualtLocation)
                        {
                            Interface.Location = new Point((Width / 2) - (Interface.Width / 2), DistanceY);
                        }
                        Core.Graphics.Focus();
                        if (!CursorVis && !Interface.Visible)
                        {
                            Cursor.Hide();
                        }
                    }
                    else
                    {
                        GISetDefualtLocation = GTISetDefualtLocation;
                        GInterface.Show();
                        GInterface.BringToFront();
                        Core.Graphics.Focus();
                        if (ISetDefualtLocation && GISetDefualtLocation)
                        {
                            Interface.Location = new Point((Width / 2) - (Interface.Width + (gap / 2)), DistanceY);
                            GInterface.Location = new Point((Width / 2) + (gap / 2), DistanceY);
                        }
                        else if (GISetDefualtLocation)
                        {
                            GInterface.Location = new Point((Width / 2) - (GInterface.Width / 2), DistanceY);
                        }
                        if (!CursorVis && !Interface.Visible)
                        {
                            Cursor.Show();
                        }
                    }
                }
                else if (e.KeyCode == Keys.M)
                {
                    if (CursorVis)
                    {
                        if (!Interface.Visible && !GInterface.Visible)
                        {
                            Cursor.Hide();
                        }
                        CursorVis = false;
                    }
                    else
                    {
                        if (!Interface.Visible && !GInterface.Visible)
                        {
                            Cursor.Show();
                        }
                        CursorVis = true;
                    }
                }
            }

            #endregion
        }
    }
}*/