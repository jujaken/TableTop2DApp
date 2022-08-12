using System;
using System.Windows.Media;
using TableTop2D.Core.Base;

namespace TableTop2D.Core
{
    internal class ColorSelectionVM : ViewModelBase
    {
        #region CurrentColor

        private SolidColorBrush _CurrentColor;
        public SolidColorBrush CurrentColor
        {
            get => _CurrentColor;
            set => SetProperty(ref _CurrentColor, value);
        }

        #endregion

        #region ARGB 

        #region A

        private byte _A;
        public byte A
        {
            get => _A;
            set
            {
                SetProperty(ref _A, value);
                UpdateCurrentColor();
            }
        }

        #endregion

        #region R

        private byte _R;
        public byte R
        {
            get => _R;
            set
            {
                SetProperty(ref _R, value);
                UpdateCurrentColor();
            }
        }

        #endregion

        #region G

        private byte _G;
        public byte G
        {
            get => _G;
            set
            {
                SetProperty(ref _G, value);
                UpdateCurrentColor();
            }
        }

        #endregion

        #region B

        private byte _B;
        public byte B
        {
            get => _B;
            set
            {
                SetProperty(ref _B, value);
                UpdateCurrentColor();
            }
        }

        #endregion

        private void UpdateCurrentColor()
        {
            CurrentColor = new SolidColorBrush
            {
                Color = Color.FromArgb(A, R, G, B)
            };

            Hex = CurrentColor.Color.R.ToString("X2")
                + CurrentColor.Color.G.ToString("X2")
                + CurrentColor.Color.B.ToString("X2");
        }

        #endregion

        #region Hex

        private string _Hex;
        public string Hex
        {
            get => _Hex;
            set
            {
                string str = value;

                try
                {
                    var r = Convert.ToByte($"{str[0]}{str[1]}", 16);
                    var g = Convert.ToByte($"{str[2]}{str[3]}", 16);
                    var b = Convert.ToByte($"{str[4]}{str[5]}", 16);

                    CurrentColor = new SolidColorBrush
                    {
                        Color = Color.FromArgb(A, r, g, b)
                    };
                }
                catch
                {

                }
                
                SetProperty(ref _Hex, value);
            }
        }

        #endregion

        #region GradientBrush

        private LinearGradientBrush _GradientBrush = new ();
        public LinearGradientBrush GradientBrush
        {
            get => _GradientBrush;
            set => SetProperty(ref _GradientBrush, value);
        }

        #endregion

        public ColorSelectionVM()
        {
            _CurrentColor = Brushes.DarkCyan;
            (A, R, G, B) = (Brushes.DarkCyan.Color.A, Brushes.DarkCyan.Color.R, Brushes.DarkCyan.Color.G, Brushes.DarkCyan.Color.B);
            _Hex = CurrentColor.Color.R.ToString("X2") 
                + CurrentColor.Color.G.ToString("X2") 
                + CurrentColor.Color.B.ToString("X2");

            _GradientBrush = new LinearGradientBrush()
            {
                EndPoint = new System.Windows.Point(1, 0),
            };

            #region GradientBrush.GradientStops

            GradientBrush.GradientStops.Add(new GradientStop(new Color() { A = 255, R = 255, G = 0, B = 0 }, 0));
            GradientBrush.GradientStops.Add(new GradientStop(new Color() { A = 255, R = 255, G = 255, B = 0 }, 0.15));
            GradientBrush.GradientStops.Add(new GradientStop(new Color() { A = 255, R = 0, G = 255, B = 0 }, 0.3));
            GradientBrush.GradientStops.Add(new GradientStop(new Color() { A = 255, R = 0, G = 255, B = 255 }, 0.45));
            GradientBrush.GradientStops.Add(new GradientStop(new Color() { A = 255, R = 0, G = 0, B = 255 }, 0.6));
            GradientBrush.GradientStops.Add(new GradientStop(new Color() { A = 255, R = 255, G = 0, B = 255 }, 0.75));
            GradientBrush.GradientStops.Add(new GradientStop(new Color() { A = 255, R = 255, G = 0, B = 0 }, 1));

            #endregion
        }
    }
}