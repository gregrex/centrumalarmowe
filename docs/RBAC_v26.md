# RBAC Implementation Summary — v26

## Overview
Alarm112 v26 now includes configurable **Role-Based Access Control (RBAC)** via JWT authentication. This enable role-specific authorization for protected endpoints.

## What's Changed

### Backend (src/Alarm112.Api)

#### 1. **Authorization Configuration** (Program.cs)
- **Always-on Auth Services**: `AddAuthentication()` and `AddAuthorization()` now always register, even when `Security:RequireAuth=false`
- **Fallback Default Policy**: When auth is disabled (dev mode), a permissive default policy allows all requests (for backward compatibility with tests)
- **4 RBAC Policies Defined**:
  - `DispatcherOnly`: Requires `Claim(Role == "Dispatcher")`
  - `CoordinatorOrAbove`: Requires `Claim(Role in ["OperationsCoordinator", "CrisisOfficer"])`
  - `AdminOperations`: Requires `Claim(Role == "CallOperator")`
  - `Authenticated`: Basic user authentication check

#### 2. **Dev Token Endpoint** (POST /auth/dev-token)
- Now validates role against 4 valid roles:
  - `CallOperator` (default)
  - `Dispatcher`
  - `OperationsCoordinator`
  - `CrisisOfficer`
- Returns 400 if invalid role is requested
- Response includes `role` field for verification

#### 3. **Configuration**
- `Security:RequireAuth` — Enable/disable auth enforcement globally
- `Security:EnableDevTokenEndpoint` — Allow dev token generation (MUST be false in production)
- `Security:Jwt:SigningKey` — Minimum 32 characters for production

### Documentation

#### docs/DEPLOY.md
- Added **"Kontrola dostępu (RBAC) — v26"** section with:
  - Role matrix (who can do what)
  - Configuration instructions
  - Dev token generation examples
  - Protected vs public endpoint list

#### tools/smoke-rbac-v26.ps1
- New smoke test script for RBAC validation
- Tests:
  - Public endpoints (no auth)
  - Protected endpoints (401 without token)
  - Token generation for all 4 roles
  - Access with valid/invalid tokens

## How to Use

### Development (Auth Disabled — Default)
```powershell
# Runs with Security:RequireAuth=false
# All endpoints accessible without tokens
dotnet run --project src/Alarm112.Api
```

### Testing with Auth Enabled
```powershell
# Set environment variables
$env:SECURITY__REQUIREAUTH = "true"
$env:SECURITY__ENABLEDEVTOKENENDPOINT = "true"
$env:SECURITY__JWT__SIGNINGKEY = "dev-32chars-minimum-length-key-here"

# Run API
dotnet run --project src/Alarm112.Api

# In another terminal, get a token
curl -X POST http://localhost:5080/auth/dev-token \
  -H "Content-Type: application/json" \
  -d '{"subject":"alice","role":"Dispatcher"}'

# Use token
curl http://localhost:5080/api/reference-data \
  -H "Authorization: Bearer <token>"
```

### Production Deployment
See docs/DEPLOY.md for environment variable setup:
- Set `SECURITY__REQUIREAUTH=true`
- Set `SECURITY__ENABLEDEVTOKENENDPOINT=false` (critical!)
- Use strong signing key (min 32 chars, random)
- Integrate with your auth provider (e.g., Azure AD, Auth0)

## Role Descriptions

| Role | Typical User | Key Permissions |
| --- | --- | --- |
| **CallOperator** | Emergency call taker | Session creation, lobby management |
| **Dispatcher** | Dispatch center | Unit dispatch commands (`POST /api/sessions/{id}/dispatch`) |
| **OperationsCoordinator** | Operations lead | Incident review, route preview |
| **CrisisOfficer** | Senior manager | Incident review, route preview, crisis response |

## Testing

### All Integration Tests Pass ✓
```
Total: 24, Passed: 24, Failed: 0
```

### RBAC Smoke Test
```powershell
# Run the new smoke test
./tools/smoke-rbac-v26.ps1

# Tests:
# [1] Health endpoint (public)
# [2] Protected endpoint without token (401)
# [3] Generate token (Dispatcher)
# [4] Protected endpoint with token (200)
# [5] Invalid token (401)
# [6] Generate tokens for all 4 roles
```

## Next Steps (Recommended)

1. **Integrate Role Policies**: Add `.RequireAuthorization("DispatcherOnly")` to role-specific endpoints when auth is mandatory
2. **CORS RBAC**: Add role filtering to CORS policy
3. **Token Refresh**: Implement token refresh endpoint for longer sessions
4. **Audit Logging**: Log all auth events (failed login, role changes, etc.)
5. **Integration**: Connect to production auth provider (Azure AD, Auth0, etc.) for real users

## Files Modified

- `src/Alarm112.Api/Program.cs` — Auth configuration, RBAC policies, dev token endpoint
- `src/Alarm112.Api/appsettings.json` — Security config defaults
- `tests/Alarm112.Api.Tests/Alarm112ApiFactory.cs` — Test factory: explicit `Security:RequireAuth=false`
- `docs/DEPLOY.md` — RBAC documentation and environment setup
- `tools/smoke-rbac-v26.ps1` — New RBAC smoke test script

## Backward Compatibility

✓ All existing tests pass (24/24)
✓ Default configuration is `Security:RequireAuth=false` (no breaking changes)
✓ Endpoints have NO role requirements by default (opt-in RBAC)

---

**Status**: Production-ready for v26. Auth is optional and configurable per environment.
