using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TableTop2D.Core.Base.Interfaces;

namespace TableTop2D.Core.WorkTable
{
    internal class ProjectTable
    {
        /// <summary> Визуальное представление самого поля </summary>
        public Canvas TableCanvas { get; private set; }

        /// <summary> Количество ячеек в ширину и высоту </summary>
        public (byte Width, byte Height) Size = (10, 10);

        /// <summary> Список всех созданных фигур </summary>
        public List<IFigure> FigureList { get; private set; } = new();

        /// <summary> Текущая позиция курсора </summary>
        public Point CursorPosition { get; private set; }
        private Border[,] _TableBorders;

        public ProjectTable(ref Canvas tableCanvas, byte width, byte height)
        {
            Size = (width, height);
            TableCanvas = tableCanvas;
            _TableBorders = new Border[Size.Width, Size.Height];
            BorderCreate();
            TableCanvas.MouseRightButtonDown += MouseRightButtonDown;
        }

        public void CreateNewFigure(ref ProjectTable workTable, IFigure figure)
        {
            new FigureCreator(ref workTable, figure);
            workTable.FigureList.Add(figure);
        }

        #region BorderCreate

        private void BorderCreate()
        {
            for (byte i = 0; i < Size.Width; i++)
                for (byte j = 0; j < Size.Height; j++)
                {
                    _TableBorders[i, j] = new Border
                    {
                        Child = new Canvas(),
                        BorderBrush = Brushes.DarkCyan,
                        Width = TableCanvas.Width / Size.Width,
                        Height = TableCanvas.Height / Size.Height,
                        BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5),
                        Margin = new Thickness(TableCanvas.Width / Size.Width * i, TableCanvas.Height / Size.Height * j, 0, 0)
                    };
                    TableCanvas.Children.Add(_TableBorders[i, j]);
                }
        }

        #endregion

        #region MouseRightButtonDown

        private void MouseRightButtonDown(object sender, MouseButtonEventArgs e) =>
            CursorPosition = e.GetPosition(TableCanvas);

        #endregion

        #region GetStartPointCell

        public Point GetPointFigureInCell(Point cursorPosition, double width, double height)
        {
            if (cursorPosition.X < 0 || cursorPosition.Y < 0)
                throw new Exception("Курсор за полем!");

            var cellLongX = TableCanvas.Width / Size.Width;
            var cellLongY = TableCanvas.Height / Size.Height;

            for (ushort x = 0; x < Size.Width; x++)
                if (cellLongX * x < cursorPosition.X && cursorPosition.X < cellLongX * (x + 1))
                    for (ushort y = 0; y < Size.Height; y++)
                        if (cellLongY * y < cursorPosition.Y && cursorPosition.Y < cellLongY * (y + 1))
                            return new Point(x * cellLongX + (cellLongX - width) / 2, y * cellLongY + (cellLongY - height) / 2);

            throw new Exception("Курсор за полем!");
        }

        #endregion
    }
}
