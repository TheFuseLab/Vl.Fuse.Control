﻿using Stride.Core.Mathematics;

namespace Fuse.Controls
{
    public class LinearControlPoint : ControlPoint
    {
        public LinearControlPoint() : base(ControlPointType.LINEAR){
            ;
        }

        public LinearControlPoint(float theTime, float theValue):base(theTime, theValue, ControlPointType.LINEAR) {
        }
	
        public LinearControlPoint(ControlPoint theControlPoint):this(theControlPoint.Time, theControlPoint.Value) {
            ;
        }
	
        /* (non-Javadoc)
         * @see de.artcom.timeline.model.ControlPoint#interpolateValue()
         */
        
        public override float InterpolateValue(float theTime, AnimationCurve theData) {
            ControlPoint mySample = new StepControlPoint(theTime,0);
            var myLower = theData.Lower(mySample);
            if(myLower.Type == ControlPointType.BEZIER) {
                var myBezierPoint = (BezierControlPoint)myLower;

                return myBezierPoint.SampleBezierSegment(myBezierPoint, myBezierPoint.OutHandle, this, this, theTime);
            }

            myLower = theData.GetLastOnSamePosition(myLower);
            var myHigher = theData.Ceiling(mySample);
	
            if (theData.Contains(mySample)) {
                return theData.TailSet(mySample, true).Min.Value;
            }
	
            if (myLower == null) {
                return myHigher.Value;
            }

            if (myHigher == null) {
                return myLower.Value;
            }
            var myBlend = (theTime - myLower.Time) / (myHigher.Time - myLower.Time);
            
            return MathUtil.Lerp(myLower.Value, myHigher.Value, myBlend);

        }

        public new LinearControlPoint Clone() {
            var myResult = new LinearControlPoint(Time, Value);
           // if(_myBlendable != null)myResult._myBlendable = (CCBlendable<?>)_myBlendable.clone();
            return myResult;
        }
    }
}