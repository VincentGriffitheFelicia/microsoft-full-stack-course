# Middleware Testing Checklist & Results

## Setup

- **Port:** https://localhost:5235
- **Valid Test Token:**
  `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c`
- **Test File:** `/requests.http`

---

## Test Execution Steps

### 1. Start the Application

```bash
dotnet run
```

Expected: Application starts on https://localhost:5235

### 2. Open requests.http in VS Code

- Install REST Client extension if not already installed
- Open `requests.http` file

### 3. Run Tests in Order

#### Phase 1: Authentication Tests (Tests 1-5)

| Test # | Description          | Expected Result  | Status |
| ------ | -------------------- | ---------------- | ------ |
| 1      | Public root endpoint | 200 OK           | ☐      |
| 2      | Missing auth header  | 401 Unauthorized | ☐      |
| 3      | Invalid scheme       | 401 Unauthorized | ☐      |
| 4      | Invalid token        | 401 Unauthorized | ☐      |
| 5      | Valid token          | 200 OK           | ☐      |

#### Phase 2: CRUD Operations (Tests 6-11)

| Test # | Description        | Expected Result | Status |
| ------ | ------------------ | --------------- | ------ |
| 6      | Create user        | 201 Created     | ☐      |
| 7      | Get all users      | 200 OK          | ☐      |
| 8      | Get user by ID     | 200 OK          | ☐      |
| 9      | Update user        | 200 OK          | ☐      |
| 10     | Create second user | 201 Created     | ☐      |
| 11     | Delete user        | 204 No Content  | ☐      |

#### Phase 3: Exception Handling (Tests 12-17)

| Test # | Description       | Expected Result | Status |
| ------ | ----------------- | --------------- | ------ |
| 12     | Invalid user ID   | 400 Bad Request | ☐      |
| 13     | Non-existent user | 404 Not Found   | ☐      |
| 14     | Invalid email     | 400 Bad Request | ☐      |
| 15     | Missing field     | 400 Bad Request | ☐      |
| 16     | Empty body        | 400 Bad Request | ☐      |
| 17     | Duplicate email   | 400 Bad Request | ☐      |

#### Phase 4: Logging Tests (Tests 18-20)

| Test # | Description        | Log Check         | Status |
| ------ | ------------------ | ----------------- | ------ |
| 18     | GET request        | Verify log output | ☐      |
| 19     | POST request       | Verify log output | ☐      |
| 20     | Final verification | All logs recorded | ☐      |

---

## Log Verification Checklist

### Exception Handling Middleware Logs

- ☐ Catches `UnauthorizedAccessException` exceptions
- ☐ Catches `ArgumentException` exceptions
- ☐ Catches general exceptions
- ☐ Returns JSON error responses
- ☐ Logs error details

### Authentication Middleware Logs

- ☐ Logs public route access
- ☐ Logs missing authorization header
- ☐ Logs invalid scheme
- ☐ Logs invalid token
- ☐ Logs valid token authentication
- ☐ Returns proper 401 responses

### Logging Middleware Logs

- ☐ Logs HTTP method (GET, POST, PUT, DELETE)
- ☐ Logs request path
- ☐ Logs response status code (200, 201, 400, 401, 404, 500)
- ☐ Logs request duration in milliseconds
- ☐ Logs timestamp for each request

---

## Error Handling Verification

### 401 Unauthorized Responses

Test missing/invalid authentication:

```json
{
    "error": "Missing authorization header"
}
```

### 400 Bad Request Responses

Test invalid data:

```json
{
    "error": "Invalid request parameters"
}
```

### 500 Internal Server Error Responses

Test unhandled exceptions:

```json
{
    "error": "Internal server error"
}
```

---

## Console Output Example

When running tests, you should see logs like:

```
Incoming Request: GET / | Timestamp: 2026-01-29T15:45:30.1234567Z
Outgoing Response: GET / | Status: 200 | Duration: 12ms

Incoming Request: GET /api/users | Timestamp: 2026-01-29T15:45:35.5678901Z
Missing authorization header for GET /api/users
Outgoing Response: GET /api/users | Status: 401 | Duration: 8ms

Incoming Request: GET /api/users | Timestamp: 2026-01-29T15:45:40.2345678Z
Valid token authenticated for GET /api/users
Outgoing Response: GET /api/users | Status: 200 | Duration: 45ms

Incoming Request: POST /api/users | Timestamp: 2026-01-29T15:45:45.3456789Z
Valid token authenticated for POST /api/users
Outgoing Response: POST /api/users | Status: 201 | Duration: 50ms
```

---

## Test Results Summary

### ✓ All Tests Passed

- [ ] All authentication tests passed
- [ ] All CRUD operations successful with valid tokens
- [ ] All invalid requests properly rejected with correct status codes
- [ ] All exception handling working correctly
- [ ] All logs being recorded accurately

### ✓ Middleware Verification

- [ ] Exception handling middleware functioning
- [ ] Authentication middleware validating tokens
- [ ] Logging middleware recording all requests/responses
- [ ] Middleware execution order correct
- [ ] Error responses in JSON format

### ✓ Security Compliance

- [ ] API rejects requests without tokens
- [ ] Invalid tokens are rejected
- [ ] Public endpoints accessible without tokens
- [ ] Protected endpoints require valid tokens
- [ ] Consistent error messages (no info leakage)

---

## Notes

- Monitor the console output during testing
- Response times should be under 100ms for most operations
- All errors should return JSON format responses
- No unhandled exceptions should reach the client

---

## Troubleshooting

### Issue: HTTPS Certificate Error

**Solution:** Accept self-signed certificate or use `https://localhost:5235`

### Issue: Port Already in Use

**Solution:** Change port in `launchSettings.json` or kill existing process

### Issue: Missing Authorization Header Not Rejected

**Solution:** Ensure AuthenticationMiddleware is registered before controllers are mapped

### Issue: Logs Not Showing

**Solution:** Check that logging is configured in Program.cs with `AddConsole()`

---

## Next Steps After Testing

1. ✓ Verify all tests pass
2. ✓ Document any failures
3. ✓ Review middleware implementation
4. ✓ Consider production security enhancements
5. ✓ Deploy to staging environment
