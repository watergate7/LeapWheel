using System;
using System.Collections.Generic;
using System.Threading;
using Leap;

namespace LeapMotionTest
{
    class SimpleWheelListener : Listener
    {
        private Object thisLock = new Object();

        private void SafeWriteLine(String line)
        {
            lock (thisLock)
            {
                Console.WriteLine(line);
            }
        }

        public override void OnInit(Controller controller)
        {
            SafeWriteLine("WheelListener Initialized");
        }

        public override void OnConnect(Controller controller)
        {
            SafeWriteLine("Device Connected");
        }

        public override void OnDisconnect(Controller controller)
        {
            //Note: not dispatched when running in a debugger.
            SafeWriteLine("Device Disconnected");
        }

        public override void OnExit(Controller controller)
        {
            SafeWriteLine("WheelListener Exited");
        }

        public override void OnFrame(Controller controller)
        {
            // Get the most recent frame and report some basic information
            Frame frame = controller.Frame();

            // Decide if the car goes forward, backward or stands still depending on the extended condition of left and right thumb
            Hand leftHand = frame.Hands[0];
            Hand rightHand = frame.Hands[1];

            FingerList extendedLeftFingerList = leftHand.Fingers.Extended();
            FingerList extendedRightFingerList = rightHand.Fingers.Extended();
            FingerList leftThumbList = extendedLeftFingerList.FingerType(Finger.FingerType.TYPE_THUMB);
            FingerList rightThumbList = extendedRightFingerList.FingerType(Finger.FingerType.TYPE_THUMB);
            Finger leftThumb = leftThumbList.Count == 0 ? null : leftThumbList[0];
            Finger rightThumb = rightThumbList.Count == 0 ? null : rightThumbList[0];

            if (leftThumb == null && rightThumb == null || leftThumb != null && rightThumb != null)
            {
                SafeWriteLine("No move");
            }
            else if (leftThumb != null && rightThumb == null)
            {
                SafeWriteLine("Fd");
            }
            else
            {
                SafeWriteLine("Bd");
            }


            // Set the direction
            Vector leftVec = leftHand.PalmPosition;
            Vector rightVec = rightHand.PalmPosition;

            float slope = (leftVec.z - rightVec.z) / (leftVec.x - rightVec.x);
            double angle = Math.Atan(slope) * 180 / Math.PI;
            angle = angle < 90 ? angle : angle - 180;

            //only if the angle between two lines is below 15, do the following
            if (Math.Abs(angle) > 10)
            {
                SafeWriteLine(angle.ToString());
            }
            else
            {
                SafeWriteLine("Mid");
            }
        }
    }

    class WheelListener : Listener
    {
        private Object thisLock = new Object();
        private float x, z;

        public WheelListener(float[] center)
        {
            if(center!=null)
            {
                x = center[0];
                z = center[1];
            }
            else
            {
                x = 0;
                z = 0;
            }
        }

        private void SafeWriteLine(String line)
        {
            lock (thisLock)
            {
                Console.WriteLine(line);
            }
        }

        public override void OnInit(Controller controller)
        {
            SafeWriteLine("WheelListener Initialized");
        }

        public override void OnConnect(Controller controller)
        {
            SafeWriteLine("Device Connected");
        }

        public override void OnDisconnect(Controller controller)
        {
            //Note: not dispatched when running in a debugger.
            SafeWriteLine("Device Disconnected");
        }

        public override void OnExit(Controller controller)
        {
            SafeWriteLine("WheelListener Exited");
        }

        public override void OnFrame(Controller controller)
        {
            // Get the most recent frame and report some basic information
            Frame frame = controller.Frame();

            // Decide if the car goes forward, backward or stands still depending on the extended condition of left and right thumb
            Hand leftHand = frame.Hands[0];
            Hand rightHand = frame.Hands[1];

            FingerList extendedLeftFingerList = leftHand.Fingers.Extended();
            FingerList extendedRightFingerList = rightHand.Fingers.Extended();
            FingerList leftThumbList = extendedLeftFingerList.FingerType(Finger.FingerType.TYPE_THUMB);
            FingerList rightThumbList = extendedRightFingerList.FingerType(Finger.FingerType.TYPE_THUMB);
            Finger leftThumb = leftThumbList.Count == 0 ? null : leftThumbList[0];
            Finger rightThumb = rightThumbList.Count == 0 ? null : rightThumbList[0];
            
            if (leftThumb == null && rightThumb == null || leftThumb != null && rightThumb != null)
            {
                SafeWriteLine("No move");
            }
            else if (leftThumb != null && rightThumb == null)
            {
                SafeWriteLine("Fd");
            }
            else
            {
                SafeWriteLine("Bd");
            }


            // Set the direction
            Vector leftVec = leftHand.PalmPosition;
            Vector rightVec = rightHand.PalmPosition;


            float slopeLeftCenter = (z - leftVec.z) / (x - leftVec.x);
            float slopeRightCenter = (z - rightVec.z) / (x - rightVec.x);
            double tanAngle = Math.Abs((slopeRightCenter - slopeLeftCenter) / (1 + slopeLeftCenter * slopeRightCenter))*180/Math.PI;
            double angle = Math.Atan(tanAngle)*180/Math.PI;

            //only if the angle between two lines is below 15, do the following
            if (angle < 15)
            {
                double leftAngle = Math.Atan(slopeLeftCenter)*180/Math.PI;
                leftAngle = leftAngle < 90 ? leftAngle : leftAngle - 180;
                double rightAngle = Math.Atan(slopeRightCenter)*180/Math.PI;
                rightAngle = rightAngle < 90 ? rightAngle : rightAngle - 180;
                double midAngle = (leftAngle + rightAngle) / 2;
                SafeWriteLine("left"+leftAngle.ToString());
                SafeWriteLine("right"+rightAngle.ToString());
                SafeWriteLine(midAngle.ToString());
            }
            else
            {
                SafeWriteLine("Mid");
            }
        }
    }

    class InitListener : Listener
    {
        private Object thisLock = new Object();
        private HandsPosUtil handsPos = new HandsPosUtil();

        private void SafeWriteLine(String line)
        {
            lock (thisLock)
            {
                Console.WriteLine(line);
            }
        }

        public override void OnInit(Controller controller)
        {
            SafeWriteLine("InitListener Initialized");
        }

        public override void OnConnect(Controller controller)
        {
            SafeWriteLine("Device Connected");
        }

        public override void OnDisconnect(Controller controller)
        {
            //Note: not dispatched when running in a debugger.
            SafeWriteLine("Device Disconnected");
        }

        public override void OnExit(Controller controller)
        {
            SafeWriteLine("InitListener Exited");
        }

        public override void OnFrame(Controller controller)
        {
            // Get the most recent frame and report some basic information
            Frame frame = controller.Frame();

            // Save the position of left and right hand
            Vector leftPos = frame.Hands[0].PalmPosition;
            Vector rightPos = frame.Hands[1].PalmPosition;
            Console.WriteLine("Left:"+leftPos.x+" "+leftPos.y+" "+leftPos.z);
            Console.WriteLine("Right:"+rightPos.x+" "+rightPos.y+" "+rightPos.z);
            handsPos.addHandsPos(leftPos, rightPos);
        }

        public float[] getInitCenter()
        {
            return handsPos.getFlatCenter();
        }
    }

    class HandsPosUtil
    {
        private List<Vector> leftHandPos = new List<Vector>();
        private List<Vector> rightHandPos = new List<Vector>();

        // Add the position of hands
        public void addHandsPos(Vector leftVec,Vector rightVec)
        {
            if (leftVec == null || rightVec == null)
                return;
            else
            {
                leftHandPos.Add(leftVec);
                rightHandPos.Add(rightVec);
            }
        }

        // Calculate the center of two hands in 2-d (x-z)
        public float[] getFlatCenter()
        {
            float centerX=0;
            float centerZ=0;
            int count = leftHandPos.Count;
            for (int i = 0; i < count; i++)
            {
                Vector leftV = leftHandPos[i];
                Vector rightV = rightHandPos[i];
                centerX += leftV.x / 2 + rightV.x / 2;
                centerZ += leftV.z / 2 + rightV.z / 2;
            }
            Console.WriteLine("Center:"+ centerX / count+ " "+ centerZ / count);
            return new float[]{centerX/count,centerZ/count};
        }
    }

    static class ConsoleRead
    {
        public static int readCommand(string command)
        {
            int input;
            do
            {
                Console.WriteLine(command);
                input = Console.ReadLine().ToCharArray()[0];
            } while (!(input == 'n' || input == 'N' || input == 'y' || input == 'Y'));

            if (input == 'n' || input == 'N')
                return 0;
            else
                return 1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a controller
            Controller controller = new Controller();

            // Init center of the wheel
            //float[] center = null;
            //int ans = ConsoleRead.readCommand("Init the center of the wheel [y/n]");
            //if (ans == 0)
            //    Environment.Exit(0);
            //else
            //{
            //    InitListener initListener = new InitListener();
            //    Console.WriteLine("Put your hands on the wheel and turn it for 5s");
            //    Thread.Sleep(1000);
            //    Console.WriteLine("Ready in 3");
            //    Thread.Sleep(1000);
            //    Console.WriteLine("Ready in 2");
            //    Thread.Sleep(1000);
            //    Console.WriteLine("Ready in 1");
            //    Thread.Sleep(1000);
            //    controller.AddListener(initListener);
            //    Thread.Sleep(5000);
            //    center = initListener.getInitCenter();
            //    controller.RemoveListener(initListener);
            //}

            //Console.ReadLine();
            ////Create WheelListener and monitor hands position
            //WheelListener wheelListener = new WheelListener(center);
            SimpleWheelListener wheelListener = new SimpleWheelListener();
            Console.WriteLine("Starting WheelMode");
            Thread.Sleep(1000);
            Console.WriteLine("Ready in 3");
            Thread.Sleep(1000);
            Console.WriteLine("Ready in 2");
            Thread.Sleep(1000);
            Console.WriteLine("Ready in 1");
            Thread.Sleep(1000);
            controller.AddListener(wheelListener);

            // Keep this process running until Enter is pressed
            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();

            // Remove the sample listener when done
            controller.RemoveListener(wheelListener);
            controller.Dispose();
        }
    }
}
