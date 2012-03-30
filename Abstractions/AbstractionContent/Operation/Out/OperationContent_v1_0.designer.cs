 
using System;

		namespace Demo.Operations { 
				public class GetPlayingPositionParameters 
		{
				public Engine engine ;
				}
		
		public class GetPlayingPosition 
		{
				private static void PrepareParameters(GetPlayingPositionParameters parameters)
		{
					GetPlayingPositionImplementation.ParameterValidation_EngineNotNull_ThrowsException(parameters.engine);
				}
				public static GetPlayingPositionReturnValue Execute(GetPlayingPositionParameters parameters)
		{
						PrepareParameters(parameters);
					int GetPlayingPositionFromPlatformOutput = GetPlayingPositionImplementation.ExecuteMethod_GetPlayingPositionFromPlatform(parameters.engine);		
				GetPlayingPositionReturnValue returnValue = GetPlayingPositionImplementation.Get_ReturnValue();
		return returnValue;
				}
				}
				public class GetPlayingPositionReturnValue 
		{
				public int PlayingPosition ;
				}
		 } 