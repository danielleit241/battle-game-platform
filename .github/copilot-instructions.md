# C# Coding Standards

## Naming
- **PascalCase**: class, method, property, interface (e.g. `UserService`, `GetUserById`).
- **camelCase**: private fields, parameters (e.g. `_userRepo`, `userId`).
- File name = Class name.

## Structure
Follow **Clean Architecture**:
```
Domain → Application → Infrastructure → WebApi
```
- Inner layers must not depend on outer layers.
- Keep methods short and single-responsibility.
- Use Dependency Injection everywhere.

## Error Handling
- Throw custom exceptions (e.g. `NotFoundException`).
- Don’t catch generic `Exception`.
- Return standardized **ProblemDetails** for API errors.

## Logging
Use `ILogger<T>`:
```csharp
_logger.LogInformation("User {UserId} retrieved", userId);
_logger.LogError(ex, "Failed to get user {UserId}", userId);
```
- Info = success, Warning = recoverable, Error = failure.
- Never log sensitive data.

## RESTful API
- Controller: plural noun (`UsersController`).
- Use correct verbs:
  - GET `/api/users`
  - POST `/api/users`
  - PUT `/api/users/{id}`
  - DELETE `/api/users/{id}`
- Return proper codes (200, 201, 204, 400, 404, 500).
- Use DTOs, not domain models.

## Readability
- Prefer LINQ over loops.
- Use `async/await` for I/O.
- Guard clauses > nested `if`.
- Comments explain **why**, not **what**.
- One class per file.

## Validation & Security
- Use `[Required]`, `[MaxLength]`, `[Range]`.
- Sanitize inputs.
- No sensitive data in logs or responses.

## Copilot Hints
- `IService` → `Service` → `Controller` pattern.
- Keep controllers thin.
- Always log and validate inputs.
- Favor small, clear, reusable methods.

---

> Goal: Write **clean, maintainable, and RESTful** C# code following **Clean Architecture** principles.
