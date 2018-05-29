FROM microsoft/dotnet:2.0.0-sdk
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

EXPOSE 9001/tcp

ENV ASPNETCORE_URLS http://0.0.0.0:9001

ENTRYPOINT ["dotnet", "out/paypart_user_gateway.dll"]