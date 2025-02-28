# CountriesController API Documentation

## Overview
This controller manages country blocking operations, including retrieving blocked countries, adding and removing blocked countries, and temporarily blocking a country.

## Base Route
`api/countries`

---

### **1. Get All Blocked Countries**
**Endpoint:** `GET /api/countries/blocked`

**Description:** Retrieves all blocked countries, with optional search and pagination.

**Query Parameters:**
- `search` (optional, string): Filters countries by name or code.
- `page` (optional, int): Specifies the page number (must be provided with `pageSize`).
- `pageSize` (optional, int): Specifies the number of results per page (must be provided with `page`).

**Responses:**
- `200 OK`: Returns a list of blocked countries.
- `400 Bad Request`: If only `page` or `pageSize` is provided (but not both).
- `404 Not Found`: If no blocked countries match the search criteria.

---

### **2. Block a Country**
**Endpoint:** `POST /api/countries/block`

**Description:** Adds a country to the blocked list.

**Request Body:**
```json
{
  "countryCode": "US",
  "countryName": "United States"
}
```

**Responses:**
- `200 OK`: Country successfully blocked.
- `400 Bad Request`: If country code or name is empty.
- `409 Conflict`: If the country is already blocked.

---

### **3. Unblock a Country**
**Endpoint:** `DELETE /api/countries/block/{countryCode}`

**Description:** Removes a country from the blocked list.

**Path Parameter:**
- `countryCode` (string, required): The country code to unblock.

**Responses:**
- `200 OK`: Country successfully unblocked.
- `404 Not Found`: If the country is not found in the blocked list.

---

### **4. Temporarily Block a Country**
**Endpoint:** `POST /api/countries/temporal-block`

**Description:** Temporarily blocks a country for a specified duration.

**Request Body:**
```json
{
  "countryCode": "US",
  "durationMinutes": 60
}
```

**Responses:**
- `200 OK`: Country successfully blocked for the given duration.
- `400 Bad Request`: If the duration is not between 1 and 1440 minutes, or if `countryCode` is missing.
- `409 Conflict`: If the country is already temporarily blocked.

---

## IPController API Documentation

## Base Route
`api/ip`

---

### **5. Get Country by IP Address**
**Endpoint:** `GET /api/ip/lookup`

**Description:** Retrieves the country code based on the provided IP address.

**Query Parameters:**
- `ip` (optional, string): The IP address to look up. If omitted, the client's IP address is used.

**Responses:**
- `200 OK`: Returns the country code for the given IP.
- `400 Bad Request`: If the IP address cannot be determined.

---

### **6. Check If IP is Blocked**
**Endpoint:** `GET /api/ip/check-block`

**Description:** Checks if the client’s IP address is blocked or temporarily blocked.

**Responses:**
- `200 OK`: Returns the country information if not blocked.
- `400 Bad Request`: If the IP address cannot be determined.
- `409 Conflict`: If the IP is blocked or temporarily blocked.

---

## LogsController API Documentation

## Base Route
`api/logs`

---

### **7. Get Blocked Attempts Logs**
**Endpoint:** `GET /api/logs/blocked-attempts`

**Description:** Retrieves a paginated list of blocked attempts.

**Query Parameters:**
- `page` (optional, int, default: 1): The page number for pagination.
- `pageSize` (optional, int, default: 10): The number of records per page.

**Responses:**
- `200 OK`: Returns a list of blocked attempts.

**Response Example:**
```json
[
  {
    "ipAddress": "192.168.1.1",
    "timestamp": "2024-02-28T12:34:56Z",
    "countryCode": "US",
    "blockedStatus": true,
    "userAgent": "Mozilla/5.0"
  }
]
```

---

## Configuration

### **AppSettings Configuration**

The application requires the following configuration in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "IpStack": {
    "IpStackUrl": "http://api.ipstack.com/",
    "IpStackKey": "PUT YOUR KEY HERE"
  }
}
```

- `IpStackUrl`: The base URL for the IP stack service.
- `IpStackKey`: API key for authentication with the IP stack service.

---

## Notes
- The API uses an in-memory storage system.
- Country codes are expected to be in uppercase format.
- Temporarily blocked countries expire automatically after the specified duration.
- The `check-block` endpoint logs all access attempts for security purposes.
- Logs provide visibility into blocked IP access attempts.
- If u need to test real IP, Un Comment the x-forwared-x code 
