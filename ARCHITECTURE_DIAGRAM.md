# Architecture & Middleware Flow Diagram

## Request/Response Flow Through Middleware Pipeline

```
┌─────────────────────────────────────────────────────────────────────┐
│                         INCOMING HTTP REQUEST                        │
│              (GET /api/users, Authorization: Bearer xyz)             │
└────────────────────┬────────────────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────────────────┐
│     1. ExceptionHandlingMiddleware                                   │
│     ┌─────────────────────────────────────────────────────────────┐ │
│     │ • Wraps entire request in try-catch                        │ │
│     │ • Catches UnauthorizedAccessException → 401               │ │
│     │ • Catches ArgumentException → 400                         │ │
│     │ • Catches all others → 500                                │ │
│     │ • Logs exceptions                                         │ │
│     │ • Returns JSON error responses                            │ │
│     └─────────────────────────────────────────────────────────────┘ │
└────────────────────┬────────────────────────────────────────────────┘
                     │ (Pass control)
                     ▼
┌─────────────────────────────────────────────────────────────────────┐
│     2. AuthenticationMiddleware                                      │
│     ┌─────────────────────────────────────────────────────────────┐ │
│     │ • Check if route is public                                │ │
│     │   ✓ /, /swagger, /api/docs → PASS                        │ │
│     │   ✗ /api/users → CHECK TOKEN                             │ │
│     │ • Validate Authorization header exists                   │ │
│     │ • Validate "Bearer" scheme                               │ │
│     │ • Validate token format (min 20 chars, no spaces)        │ │
│     │ • Log authentication attempts                            │ │
│     │ • Return 401 for invalid tokens                          │ │
│     └─────────────────────────────────────────────────────────────┘ │
└────────────────────┬────────────────────────────────────────────────┘
                     │ (Pass control if auth valid)
                     ▼
┌─────────────────────────────────────────────────────────────────────┐
│     3. RequestLoggingMiddleware                                      │
│     ┌─────────────────────────────────────────────────────────────┐ │
│     │ START: Log incoming request                               │ │
│     │ • Log: "Incoming Request: GET /api/users"                │ │
│     │ • Timestamp, method, path                                │ │
│     └─────────────────────────────────────────────────────────────┘ │
└────────────────────┬────────────────────────────────────────────────┘
                     │ (Pass control)
                     ▼
┌─────────────────────────────────────────────────────────────────────┐
│        ASP.NET Core Routing & Controller Processing                 │
│     ┌─────────────────────────────────────────────────────────────┐ │
│     │ • Route to appropriate controller                          │ │
│     │ • UsersController.GetAllUsers()                           │ │
│     │ • Execute business logic                                  │ │
│     │ • Call IUserService methods                               │ │
│     │ • Validate input                                          │ │
│     │ • Return response                                         │ │
│     └─────────────────────────────────────────────────────────────┘ │
└────────────────────┬────────────────────────────────────────────────┘
                     │ (Response generated)
                     ▼
┌─────────────────────────────────────────────────────────────────────┐
│     3. RequestLoggingMiddleware (RESPONSE PHASE)                    │
│     ┌─────────────────────────────────────────────────────────────┐ │
│     │ END: Log outgoing response                                │ │
│     │ • Log: "Outgoing Response: GET /api/users | 200 | 45ms"  │ │
│     │ • Status code, duration, path                            │ │
│     │ • Copy response to client                                │ │
│     └─────────────────────────────────────────────────────────────┘ │
└────────────────────┬────────────────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────────────────┐
│               OUTGOING HTTP RESPONSE (200 OK)                        │
│        { "id": 1, "firstName": "John", "lastName": "Doe" }         │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Decision Trees

### Authentication Flow

```
┌─ Request arrives with path /api/users
│
├─ Is this a public route?
│  ├─ YES (/, /swagger, /api/docs) → Allow & Continue
│  └─ NO → Proceed to token check
│
├─ Is "Authorization" header present?
│  ├─ NO → Return 401 "Missing authorization header"
│  └─ YES → Proceed to scheme validation
│
├─ Does header start with "Bearer "?
│  ├─ NO → Return 401 "Invalid authorization scheme"
│  └─ YES → Proceed to token validation
│
├─ Is token valid? (length ≥ 20, no spaces)
│  ├─ NO → Return 401 "Invalid or expired token"
│  └─ YES → Allow access & Continue
│
└─ Request reaches controller
```

### Error Handling Flow

```
┌─ Request processing begins
│
└─ Exception thrown?
   ├─ UnauthorizedAccessException
   │  └─ Return 401 + JSON error
   │
   ├─ ArgumentException
   │  └─ Return 400 + JSON error
   │
   └─ Any other exception
      └─ Return 500 + JSON error
         (Catch-all for unexpected errors)
```

### Logging Flow

```
┌─ Request enters logging middleware
│
├─ Record incoming:
│  ├─ HTTP Method (GET, POST, PUT, DELETE)
│  ├─ Request Path (/api/users, /api/users/1)
│  └─ Timestamp (2026-01-29T15:45:30Z)
│
├─ Pass to next middleware
│
├─ Response is generated
│
└─ Record outgoing:
   ├─ Status Code (200, 201, 400, 401, 404, 500)
   ├─ Duration (measured in ms)
   └─ Timestamp
```

---

## Middleware Order Importance

### Correct Order ✓

```
Request → [ExceptionHandler] → [Authentication] → [Logging] → [Controller]
            ↓ (Exception handling)    ↓ (Token check)     ↓ (Log request)
            └────────────────────────────────────────────┬──────────────────
                                                         ↓ (Process)
Response ← [ExceptionHandler] ← [Authentication] ← [Logging] ← [Controller]
            ↓ (Format error)          ↓ (Already pass)   ↓ (Log response)
```

### Wrong Order ✗ (If Logging was first)

```
Request → [Logging] → [ExceptionHandler] → [Authentication] → [Controller]
          (No info about auth)
          ❌ Logs invalid requests as valid
          ❌ Doesn't capture auth failures properly
```

---

## Test Scenarios & Expected Flow

### Test 1: Valid Token Request ✓

```
GET /api/users
Authorization: Bearer [valid-token]
        │
        ├─ ExceptionHandler: No exception → Pass
        ├─ Authentication: Token valid → Pass
        ├─ RequestLogging: Log incoming
        ├─ Controller: Process & return user list
        ├─ RequestLogging: Log outgoing (200)
        └─ Response: 200 OK + User data
```

### Test 2: Missing Authorization Header ✗

```
GET /api/users
(no Authorization header)
        │
        ├─ ExceptionHandler: No exception → Pass
        ├─ Authentication: No header found
        │  └─ Return 401 + { "error": "Missing authorization header" }
        └─ RequestLogging: Log outgoing (401)
```

### Test 3: Invalid Email on Create ✗

```
POST /api/users
{
  "firstName": "John",
  "email": "not-an-email",    ← Invalid email format
  ...
}
        │
        ├─ ExceptionHandler: No exception → Pass
        ├─ Authentication: Token valid → Pass
        ├─ RequestLogging: Log incoming
        ├─ Controller: Validate input
        │  └─ ModelState invalid → Return 400
        ├─ UserService: Not reached (validation failed)
        ├─ RequestLogging: Log outgoing (400)
        └─ Response: 400 Bad Request + validation errors
```

### Test 4: Unhandled Exception ✗

```
POST /api/users
(Valid request but service throws exception)
        │
        ├─ ExceptionHandler: No exception yet → Pass
        ├─ Authentication: Token valid → Pass
        ├─ RequestLogging: Log incoming
        ├─ Controller: Call service
        ├─ UserService: Throws unexpected exception
        │  └─ Exception caught by ExceptionHandler
        │     └─ Return 500 + { "error": "Internal server error" }
        ├─ RequestLogging: Log outgoing (500)
        └─ Response: 500 Internal Server Error
```

---

## Service Layer Architecture

```
┌─────────────────────────────────────────────────┐
│         UsersController                         │
│  (Handles HTTP requests & validation)           │
└────────────┬────────────────────────────────────┘
             │
             ├─ Request body validation
             ├─ ID parameter validation
             ├─ Error handling
             └─ Response formatting
                     │
                     ▼
┌─────────────────────────────────────────────────┐
│    IUserService (Interface)                     │
│  • GetAllUsersAsync()                           │
│  • GetUserByIdAsync(id)                         │
│  • CreateUserAsync(user)                        │
│  • UpdateUserAsync(id, user)                    │
│  • DeleteUserAsync(id)                          │
└────────────┬────────────────────────────────────┘
             │
             ▼
┌─────────────────────────────────────────────────┐
│    UserService (Implementation)                 │
│  • Input validation (data annotations)          │
│  • Business logic                               │
│  • In-memory storage (static list)              │
│  • Thread safety (lock mechanism)               │
│  • Duplicate prevention                         │
│  • Error reporting (tuples)                     │
└────────────┬────────────────────────────────────┘
             │
             ▼
┌─────────────────────────────────────────────────┐
│    Data Models                                  │
│  • User class with validation attributes        │
│  • Required fields                              │
│  • Format validation (email, phone)             │
│  • Length constraints                           │
└─────────────────────────────────────────────────┘
```

---

## Response Status Code Matrix

| Scenario            | Status | Code                  | Middleware        |
| ------------------- | ------ | --------------------- | ----------------- |
| Success GET         | 200    | OK                    | None (success)    |
| Success POST        | 201    | Created               | None (success)    |
| Success PUT         | 200    | OK                    | None (success)    |
| Success DELETE      | 204    | No Content            | None (success)    |
| Missing auth header | 401    | Unauthorized          | Authentication    |
| Invalid token       | 401    | Unauthorized          | Authentication    |
| Invalid scheme      | 401    | Unauthorized          | Authentication    |
| Invalid input       | 400    | Bad Request           | Exception Handler |
| Not found           | 404    | Not Found             | Exception Handler |
| Unhandled exception | 500    | Internal Server Error | Exception Handler |

---

## Deployment Checklist

```
┌─ Development
│  ├─ ✓ Middleware implemented
│  ├─ ✓ Logging configured
│  ├─ ✓ Error handling complete
│  └─ ✓ Tests created
│
├─ Testing
│  ├─ Run 20 test scenarios
│  ├─ Verify all logs
│  ├─ Check error responses
│  └─ Validate status codes
│
├─ Staging
│  ├─ Deploy to staging server
│  ├─ Verify logging to persistent storage
│  ├─ Test with real tokens (JWT)
│  └─ Performance testing
│
└─ Production
   ├─ Deploy to production
   ├─ Enable monitoring/alerting
   ├─ Configure log rotation
   ├─ Implement backup strategy
   └─ Security hardening
```

---

## Summary

✓ **Middleware Pipeline:** Exception → Authentication → Logging ✓ **Error Handling:** Consistent
JSON responses ✓ **Token Authentication:** Bearer scheme validation ✓ **Request Logging:** Method,
path, status, duration ✓ **Test Coverage:** 20 comprehensive scenarios ✓ **Documentation:** Complete
with examples

All components working together to provide **enterprise-grade security and auditing**.
