#!/bin/bash

# Start the Consumer application in the background
dotnet TokenClaimConsumer.Api.dll &

# Start the Generate application in the background
dotnet TokenClaimProvider.Api.dll &

# Wait for all background jobs to complete
wait
