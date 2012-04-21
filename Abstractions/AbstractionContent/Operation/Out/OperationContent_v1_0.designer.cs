 
using System;

		namespace Demo.Operations { 
				public class SortNumbersEvensBeforeOddsParameters 
		{
				public int[] DataArray ;
				public bool AscendingOrder ;
				}
		
		public class SortNumbersEvensBeforeOdds 
		{
				private static void PrepareParameters(SortNumbersEvensBeforeOddsParameters parameters)
		{
					SortNumbersEvensBeforeOddsImplementation.ParameterValidation_DataArrayNotNull_ThrowsException(parameters.DataArray);
				}
				public static SortNumbersEvensBeforeOddsReturnValue Execute(SortNumbersEvensBeforeOddsParameters parameters)
		{
						PrepareParameters(parameters);
					int[] SortNumbersOutput = SortNumbersEvensBeforeOddsImplementation.ExecuteMethod_SortNumbers(parameters.DataArray, parameters.AscendingOrder);		
				int[] SortEvensBeforeOddsOutput = SortNumbersEvensBeforeOddsImplementation.ExecuteMethod_SortEvensBeforeOdds(SortNumbersOutput);		
				SortNumbersEvensBeforeOddsReturnValue returnValue = SortNumbersEvensBeforeOddsImplementation.Get_ReturnValue(SortEvensBeforeOddsOutput);
		return returnValue;
				}
				}
				public class SortNumbersEvensBeforeOddsReturnValue 
		{
				public int[] SortedArray ;
				}
		 } 