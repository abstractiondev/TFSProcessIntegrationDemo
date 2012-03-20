 

using System;

	namespace AbstractionBuilder {
	partial class Builder {
        public void Build()
        {
			CleanUp();
            Tuple<string, string>[] generatorFiles = null;
				FetchTransformationSources("OperationToStatusTracking", "Operation");
        generatorFiles = ExecuteAssemblyGenerator("OperationToStatusTracking", "TRANS", "Transformer");
        WriteGeneratorFiles(generatorFiles, "OperationToStatusTracking", "TRANS");
		PushTransformationTargets("OperationToStatusTracking", "StatusTracking");
				FetchTransformationSources("OperationToDocumentation", "Operation");
        generatorFiles = ExecuteAssemblyGenerator("OperationToDocumentation", "TRANS", "Transformer");
        WriteGeneratorFiles(generatorFiles, "OperationToDocumentation", "TRANS");
		PushTransformationTargets("OperationToDocumentation", "Documentation");
				FetchTransformationSources("StatusTrackingToDocumentation", "StatusTracking");
        generatorFiles = ExecuteAssemblyGenerator("StatusTrackingToDocumentation", "TRANS", "Transformer");
        WriteGeneratorFiles(generatorFiles, "StatusTrackingToDocumentation", "TRANS");
		PushTransformationTargets("StatusTrackingToDocumentation", "Documentation");
				FetchTransformationSources("ChangeRequestToDocumentation", "ChangeRequest");
        generatorFiles = ExecuteAssemblyGenerator("ChangeRequestToDocumentation", "TRANS", "Transformer");
        WriteGeneratorFiles(generatorFiles, "ChangeRequestToDocumentation", "TRANS");
		PushTransformationTargets("ChangeRequestToDocumentation", "Documentation");
			        generatorFiles = ExecuteAssemblyGenerator("Documentation", "ABS", "DesignDocumentation_v1_0");
	        WriteGeneratorFiles(generatorFiles, "Documentation", "ABS");
		        }
		
		private void CleanUp()
		{
		            CleanUpTransformationInputAndOutput("OperationToStatusTracking", "StatusTracking");
				            CleanUpTransformationInputAndOutput("OperationToDocumentation", "Documentation");
				            CleanUpTransformationInputAndOutput("StatusTrackingToDocumentation", "Documentation");
				            CleanUpTransformationInputAndOutput("ChangeRequestToDocumentation", "Documentation");
				            CleanUpAbstractionOutput("Documentation");
						}
	}
}
		