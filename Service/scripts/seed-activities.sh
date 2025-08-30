#!/usr/bin/env bash
set -euo pipefail

# Usage: ./scripts/seed-activities.sh [BASE_URL] [COUNT]
#  BASE_URL (optional): API base URL. Defaults to http://localhost:5001
#  COUNT (optional): Number of activities to create. Defaults to 10

BASE_URL="${1:-${API_BASE_URL:-http://localhost:5001}}"
COUNT="${2:-10}"

endpoint="${BASE_URL%/}/api/activities"

echo "Seeding ${COUNT} activities to ${endpoint}"

new_suffix() {
  if command -v uuidgen >/dev/null 2>&1; then
    uuidgen | tr '[:upper:]' '[:lower:]' | cut -c1-8
  else
    # Fallback: time + random
    echo "$(date +%s%N)${RANDOM}" | sha1sum | cut -c1-8
  fi
}

for i in $(seq 1 "$COUNT"); do
  suffix="$(new_suffix)"
  code=$(printf "ACT%03d-%s" "$i" "$suffix")
  name=$(printf "Activity %03d" "$i")
  desc=$(printf "Seeded activity #%03d" "$i")

  payload=$(printf '{"activityCode":"%s","activityName":"%s","description":"%s","isActive":true}' \
    "$code" "$name" "$desc")

  # Perform request, capturing body and status code without external deps like jq
  response=$(curl -sS -H "Content-Type: application/json" -H "Accept: application/json" \
    -d "$payload" -w "\n%{http_code}" "$endpoint")

  status=$(echo "$response" | tail -n1)
  body=$(echo "$response" | sed '$d')

  if [[ "$status" == "201" || "$status" == "200" ]]; then
    echo "[$i/$COUNT] Created: code=$code status=$status"
  else
    echo "[$i/$COUNT] ERROR: Failed to create code=$code status=$status"
    echo "Response body: $body"
    exit 1
  fi

done

echo "Done."
