SOLUTION_FILE = ./Brackeys-15.sln

check-format:
	dotnet format --verify-no-changes ${SOLUTION_FILE}

format:
	dotnet format ${SOLUTION_FILE}
