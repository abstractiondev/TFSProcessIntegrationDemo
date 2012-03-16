@echo Replacing abstraction module %1 with base URL source %2
call gitRemoveSubmodule.cmd %1
call gitAddNamedSub.cmd %2 %1