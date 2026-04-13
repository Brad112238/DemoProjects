#!/bin/bash

# 背景啟動 SQL Server
/opt/mssql/bin/sqlservr &

# 等待 SQL Server 就緒
echo "Waiting for SQL Server to start..."
for i in {1..30}; do
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "SELECT 1" > /dev/null 2>&1
    if [ $? -eq 0 ]; then
        echo "SQL Server is ready."
        break
    fi
    sleep 1
done

# 執行初始化 SQL
echo "Running init.sql..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -i /docker-entrypoint-initdb/init.sql

echo "Init complete."

# 保持前景
wait
