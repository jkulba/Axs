import sys

def validate_connection(connection_string):
    """Connect to the database and query the AccessRequest table"""
    try:
        try:
            import pyodbc
        except ImportError:
            print("ERROR: pyodbc module not installed")
            print("Please install it with: sudo yum install python3-pyodbc")
            return False
            
        # Connect to the database
        try:
            conn = pyodbc.connect(connection_string)
        except pyodbc.Error as e:
            if "IM002" in str(e):
                print("\nERROR: SQL Server ODBC driver not found or not properly configured.")
                print("You need to install the Microsoft ODBC driver for SQL Server on your RHEL system:")
                print("\n1. Register Microsoft repository:")
                print("   sudo curl https://packages.microsoft.com/config/rhel/8/prod.repo > /etc/yum.repos.d/mssql-release.repo")
                print("\n2. Install the driver:")
                print("   sudo yum install -y msodbcsql17")
                print("   (Accept the license terms when prompted)")
                print("\n3. Install the development package if needed:")
                print("   sudo yum install -y unixODBC-devel")
                print("\nAfter installation, try this modified connection string:")
                print("   Driver={ODBC Driver 17 for SQL Server};Server=192.168.86.45,1433;Database=AccessDb;Uid=sa;Pwd=P@ssword92;TrustServerCertificate=yes;")
            raise
            
        cursor = conn.cursor()
        
        # Execute a simple query
        print("Executing query: SELECT * FROM AccessRequest")
        cursor.execute("SELECT * FROM AccessRequest")
        
        # Fetch column names
        columns = [column[0] for column in cursor.description]
        print(f"Columns: {', '.join(columns)}")
        
        # Fetch and display results
        rows = cursor.fetchall()
        if rows:
            print(f"Found {len(rows)} records in AccessRequest table")
            
            # Display first 5 rows
            max_display = min(5, len(rows))
            print(f"\nShowing first {max_display} records:")
            for i, row in enumerate(rows[:max_display]):
                print(f"Row {i+1}:")
                for j, value in enumerate(row):
                    print(f"  {columns[j]}: {value}")
                print()
        else:
            print("AccessRequest table is empty")
            
        # Close the connection
        cursor.close()
        conn.close()
        
        return True
    except Exception as e:
        print(f"Connection error: {e}")
        return False

if __name__ == "__main__":
    print("Validating connection to AccessDb...")
    
    # For Linux/RHEL, we need to specify the driver explicitly
    is_linux = sys.platform.startswith('linux')
    
    if is_linux:
        # Linux connection string with explicit driver
        connection_string = "Driver={ODBC Driver 17 for SQL Server};Server=localhost,1433;Database=AccessDb;Uid=sa;Pwd=P@ssword92;TrustServerCertificate=yes;"
    else:
        # Windows connection string
        connection_string = "Server=localhost,1433;Database=AccessDb;User Id=sa;Password=P@ssword92;TrustServerCertificate=True;"
    
    # Display connection string with password masked
    masked_conn_string = connection_string
    if "Pwd=" in masked_conn_string:
        start = masked_conn_string.find("Pwd=") + 4
        end = masked_conn_string.find(";", start) if ";" in masked_conn_string[start:] else len(masked_conn_string)
        masked_conn_string = masked_conn_string[:start] + "********" + masked_conn_string[end:]
    elif "Password=" in masked_conn_string:
        start = masked_conn_string.find("Password=") + 9
        end = masked_conn_string.find(";", start) if ";" in masked_conn_string[start:] else len(masked_conn_string)
        masked_conn_string = masked_conn_string[:start] + "********" + masked_conn_string[end:]
    
    print(f"Using connection string: {masked_conn_string}")
    
    success = validate_connection(connection_string)
    
    if success:
        print("\nConnection validation successful!")
    else:
        print("\nConnection validation failed!")
        sys.exit(1)