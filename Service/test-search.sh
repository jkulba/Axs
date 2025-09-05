#!/bin/bash
echo "Testing search endpoint..."
curl -i "http://localhost:5001/api/access-requests/search"
echo -e "\n\nTesting with parameters..."
curl -i "http://localhost:5001/api/access-requests/search?userName=test&jobNumber=123"
