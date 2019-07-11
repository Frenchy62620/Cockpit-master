using Caliburn.Micro;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cockpit.GUI.Views.Main.Profile
{
    public class CalibrationPointCollectionDouble : PropertyChangedBase, ICollection<CalibrationPointDouble>
    {

        private LinkedList<CalibrationPointDouble> Points = new LinkedList<CalibrationPointDouble>();
        public CalibrationPointCollectionDouble()
        {
            Add(new CalibrationPointDouble(0, 0));
            Add(new CalibrationPointDouble(100, 1));
        }

        public CalibrationPointCollectionDouble(double minimumInput, double minimumOuput, double maximumInput, double maximumOutput)
        {
            Add(new CalibrationPointDouble(minimumInput, minimumOuput));
            Add(new CalibrationPointDouble(maximumInput, maximumOutput));
        }


        public double Interpolate(double input)
        {
            double retValue = 0.0d;

            if (Points.Count >= 2)
            {
                LinkedListNode<CalibrationPointDouble> valuePoint = Points.First;
                while (valuePoint != null && valuePoint.Value.Value < input)
                {
                    valuePoint = valuePoint.Next;
                }

                if (valuePoint == null)
                {
                    retValue = Points.Last.Value.Multiplier;
                }
                else if (valuePoint.Value.Value == input || valuePoint.Previous == null)
                {
                    retValue = valuePoint.Value.Multiplier;
                }
                else
                {
                    double valueRange = valuePoint.Value.Value - valuePoint.Previous.Value.Value;
                    double modifierRange = valuePoint.Value.Multiplier - valuePoint.Previous.Value.Multiplier;
                    double offsetValue = input - valuePoint.Previous.Value.Value;
                    double offsetMultiplier = offsetValue / valueRange;
                    double modifier = modifierRange * offsetMultiplier;

                    retValue = valuePoint.Previous.Value.Multiplier + modifier;
                }

                //retValue = Math.Round(retValue, Precision);
            }

            return retValue;
        }

        public double InterpolateReverse(double input)
        {
            double retValue = 0.0d;

            if (Points.Count >= 2)
            {
                LinkedListNode<CalibrationPointDouble> valuePoint = Points.First;
                while (valuePoint != null && valuePoint.Value.Multiplier < input)
                {
                    valuePoint = valuePoint.Next;
                }

                if (valuePoint == null)
                {
                    retValue = Points.Last.Value.Value;
                }
                else if (valuePoint.Value.Multiplier == input || valuePoint.Previous == null)
                {
                    retValue = valuePoint.Value.Value;
                }
                else
                {
                    double valueRange = valuePoint.Value.Multiplier - valuePoint.Previous.Value.Multiplier;
                    double modifierRange = valuePoint.Value.Value - valuePoint.Previous.Value.Value;
                    double offsetValue = input - valuePoint.Previous.Value.Multiplier;
                    double offsetMultiplier = offsetValue / valueRange;
                    double modifier = modifierRange * offsetMultiplier;

                    retValue = valuePoint.Previous.Value.Value + modifier;
                }

                //retValue = Math.Round(retValue, 2);
            }

            return retValue;
        }

        #region ICollection<KeyPoint> Members
        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(CalibrationPointDouble item)
        {
            if (item.Multiplier < OutputLimitMin) item.Multiplier = OutputLimitMin;
            if (item.Multiplier > OutputLimitMax) item.Multiplier = OutputLimitMax;

            LinkedListNode<CalibrationPointDouble> addBefore = Points.First;
            while (addBefore != null && addBefore.Value.Value < item.Value)
            {
                addBefore = addBefore.Next;
            }

            if (addBefore == null)
            {
                Points.AddLast(item);
            }
            else
            {
                Points.AddBefore(addBefore, item);
            }

            //item.PropertyChanged += Item_PropertyChanged;

            //OnCalibrationChanged();
            //OnCollectionChanged();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(CalibrationPointDouble item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(CalibrationPointDouble[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<CalibrationPointDouble> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(CalibrationPointDouble item)
        {
            throw new NotImplementedException();
        }

        #endregion

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private double _outputLimitMin = double.MinValue;
        public double OutputLimitMin
        {
            get => _outputLimitMin;
            set
            {
                if (_outputLimitMin != value)
                {
                    _outputLimitMin = value;
                    if (MinimumOutputValue < _outputLimitMin)
                    {
                        MinimumOutputValue = _outputLimitMin;
                    }
                }
            }
        }

        private double _outputLimitMax = double.MaxValue;
        public double OutputLimitMax
        {
            get => _outputLimitMax;
            set
            {
                if (_outputLimitMax != value)
                {
                    _outputLimitMax = value;
                    if (MaximumOutputValue > _outputLimitMax)
                    {
                        MaximumOutputValue = _outputLimitMax;
                    }
                }
            }
        }

        public int Precision { get; set; } = 1;

        public double MaximumInputValue
        {
            get => Points.Last.Value.Value;
            set
            {
                double oldValue = Points.Last.Value.Value;
                double lastValue = value;
                LinkedListNode<CalibrationPointDouble> node = Points.Last.Previous;
                while (node != null && node.Value.Value > lastValue)
                {
                    lastValue -= 0.1f;
                    node.Value.Value = lastValue;
                    node = node.Previous;
                }
                Points.Last.Value.Value = value;
                //OnCalibrationChanged();
                //OnPropertyChanged("MaximumInputValue", oldValue, value, false);
            }
        }

        public double MaximumOutputValue
        {
            get => Points.Last.Value.Multiplier;
            set
            {
                double oldValue = Points.Last.Value.Multiplier;
                double lastValue = value;
                //LinkedListNode<CalibrationPointDouble> node = _points.Last.Previous;
                //while (node != null && node.Value.Multiplier > lastValue)
                //{
                //    lastValue -= 0.1f;
                //    node.Value.Multiplier = lastValue;
                //    node = node.Previous;
                //}
                Points.Last.Value.Multiplier = value;
                //OnCalibrationChanged();
                //OnPropertyChanged("MaximumOutputValue", oldValue, value, false);
            }
        }

        public double MinimumInputValue
        {
            get => Points.First.Value.Value;
            set
            {
                //double oldValue = Points.First.Value.Value;
                //double lastValue = value;
                //LinkedListNode<CalibrationPointDouble> node = _points.First.Next;
                //while (node != null && node.Value.Value < lastValue)
                //{
                //    lastValue += 0.1f;
                //    node.Value.Value = lastValue;
                //    node = node.Next;
                //}
                Points.First.Value.Value = value;
            }
        }

        public double MinimumOutputValue
        {
            get => Points.First.Value.Multiplier;
            set
            {
                double oldValue = Points.First.Value.Multiplier;
                double lastValue = value;
                LinkedListNode<CalibrationPointDouble> node = Points.First.Next;
                while (node != null && node.Value.Value < lastValue)
                {
                    lastValue += 1;
                    node.Value.Multiplier = lastValue;
                    node = node.Next;
                }
                Points.First.Value.Multiplier = value;
            }
        }
    }
}
