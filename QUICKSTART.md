# Quick Start Guide - Testing Middleware

## 5-Minute Setup

### Step 1: Start the Application (30 seconds)

```bash
cd "Course Repository"
dotnet run
```

Expected output:

```
Building...
[Information] Now listening on: https://localhost:5235
```

### Step 2: Open REST Client (30 seconds)

1. Open VS Code
2. Open `requests.http` file
3. Ensure REST Client extension is installed

### Step 3: Run Tests in Order (3 minutes)

#### Phase 1: Authentication (1 minute)

```
✓ Test 1:  Root endpoint (public)
✓ Test 2:  No auth header (should fail)
✓ Test 3:  Invalid scheme (should fail)
✓ Test 4:  Invalid token (should fail)
✓ Test 5:  Valid token (should pass)
```

#### Phase 2: CRUD Operations (1 minute)

```
✓ Test 6:  Create user
✓ Test 7:  Get all users
✓ Test 8:  Get user by ID
✓ Test 9:  Update user
✓ Test 10: Create second user
✓ Test 11: Delete user
```

#### Phase 3: Error Handling (30 seconds)

```
✓ Test 12-17: Various error scenarios
```

#### Phase 4: Logging (30 seconds)

```
✓ Test 18-20: Verify logs in console
```

---

## One-Minute Summary

### What Was Built

- **3 Middleware Components:**
    1. Exception Handling - Catches errors, returns JSON
    2. Authentication - Validates Bearer tokens
    3. Logging - Records all requests/responses

### How It Works

```
Request → Exception Handler → Authentication → Logging → Controller
  ↓           (Catches errors)  (Checks token)  (Logs request)  (Processes)
Response ← Exception Handler ← Authentication ← Logging ← Controller
  ↓           (Format error)      (Already pass)  (Logs response)
```

### Key Features

✓ Token validation (Bearer scheme) ✓ Public routes (no auth needed) ✓ Consistent error responses
(JSON) ✓ Complete request/response logging ✓ Automatic exception handling ✓ Timestamp and duration
tracking

---

## Test Results Expected

### ✓ All Tests Should Pass

| Category       | Count | Expected                  |
| -------------- | ----- | ------------------------- |
| Authentication | 5     | 2 pass, 3 fail (expected) |
| CRUD           | 6     | All pass                  |
| Error Handling | 6     | All fail (expected)       |
| Logging        | 3     | All logged                |

### ✓ Console Shows Logs Like:

```
Incoming Request: GET /api/users | Timestamp: 2026-01-29...
Outgoing Response: GET /api/users | Status: 200 | Duration: 45ms
```

---

## Test Token

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

---

## Common Issues & Fixes

| Issue                   | Fix                                           |
| ----------------------- | --------------------------------------------- |
| HTTPS Certificate Error | Accept certificate warning                    |
| Port 5235 in use        | Kill existing process or change port          |
| Tests return 404        | Check URL matches `https://localhost:5235`    |
| No logs showing         | Verify Program.cs has `.AddConsole()` logging |
| 401 on all requests     | Check you're using valid token from above     |

---

## Files to Check

- ✓ `Middleware/ExceptionHandlingMiddleware.cs` - Exception handling
- ✓ `Middleware/AuthenticationMiddleware.cs` - Token validation
- ✓ `Middleware/RequestLoggingMiddleware.cs` - Request logging
- ✓ `Program.cs` - Middleware registration
- ✓ `requests.http` - All 20 tests

---

## Success = ✓

- [ ] App starts without errors
- [ ] Test 1 (public) returns 200
- [ ] Test 2 (no auth) returns 401
- [ ] Test 5 (valid token) returns 200
- [ ] Test 6 (create) returns 201
- [ ] Console shows request/response logs
- [ ] All responses in JSON format
- [ ] No unhandled exceptions

---

## Next Steps After Testing

1. Review logs in console
2. Check MIDDLEWARE_GUIDE.md for details
3. Check TESTING_CHECKLIST.md for comprehensive testing
4. Review ARCHITECTURE_DIAGRAM.md for flow diagrams
5. Consider production enhancements

---

**Everything is ready to test! Run `dotnet run` and open requests.http**
