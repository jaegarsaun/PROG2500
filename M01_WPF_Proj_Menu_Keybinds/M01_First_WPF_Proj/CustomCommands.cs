using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace M01_First_WPF_Proj
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand RandomHair = new RoutedUICommand(
            "Random Hair",
            "RandomHair",
            typeof(CustomCommands),
            new InputGestureCollection() { new KeyGesture(Key.H, ModifierKeys.Control) });

        public static readonly RoutedUICommand RandomEyes = new RoutedUICommand(
            "Random Eyes",
            "RandomEyes",
            typeof(CustomCommands),
            new InputGestureCollection() { new KeyGesture(Key.E, ModifierKeys.Control) });

        public static readonly RoutedUICommand RandomMouth = new RoutedUICommand(
            "Random Mouth",
            "RandomMouth",
            typeof(CustomCommands),
            new InputGestureCollection() { new KeyGesture(Key.M, ModifierKeys.Control) });

        public static readonly RoutedUICommand RandomNose = new RoutedUICommand(
            "Random Nose",
            "RandomNose",
            typeof(CustomCommands),
            new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control) });

        public static readonly RoutedUICommand RandomHead = new RoutedUICommand(
            "Random Head",
            "RandomHead",
            typeof(CustomCommands),
            new InputGestureCollection() { new KeyGesture(Key.J, ModifierKeys.Control) });

        public static readonly RoutedUICommand RandomFace = new RoutedUICommand(
            "Random Face",
            "RandomFace",
            typeof(CustomCommands),
            new InputGestureCollection() { new KeyGesture(Key.G, ModifierKeys.Control) });
    }
}
