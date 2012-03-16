 

using System;

	namespace AbstractionBuilder {
	partial class Builder {
        public void Build()
        {
			CleanUp();
            Tuple<string, string>[] generatorFiles = null;
				FetchTransformationSources("StatusTrackingToDocumentation", "StatusTracking");
        generatorFiles = ExecuteAssemblyGenerator("StatusTrackingToDocumentation", "TRANS", "Transformer");
        WriteGeneratorFiles(generatorFiles, "StatusTrackingToDocumentation", "TRANS");
		PushTransformationTargets("StatusTrackingToDocumentation", "Documentation");
			        generatorFiles = ExecuteAssemblyGenerator("Documentation", "ABS", "DesignDocumentation_v1_0");
	        WriteGeneratorFiles(generatorFiles, "Documentation", "ABS");
		        }
		
		private void CleanUp()
		{
		            CleanUpTransformationInputAndOutput("StatusTrackingToDocumentation", "Documentation");
				            CleanUpAbstractionOutput("Documentation");
						}
	}
}
		