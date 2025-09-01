# Signup
curl -X POST http://localhost:5210/auth/signup -H "Content-Type: application/json" \
  -d '{"username":"john","email":"john@doe.com","phoneNumber":"000","password":"Strong!Passw0rd"}'

# Login -> r�cup�re "token"
curl -X POST http://localhost:5210/auth/login -H "Content-Type: application/json" \
  -d '{"usernameOrEmail":"john","password":"Strong!Passw0rd"}'

# Cr�er un membre
curl -X POST http://localhost:5210/api/people -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" -d '{"firstName":"Alice","gender":"F"}'

# Lier parent-of
curl -X POST http://localhost:5210/api/relationships/parent-of -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" -d '{"parentId":"<GUID1>","childId":"<GUID2>"}'

# Liste anc�tres
curl -X GET "http://localhost:5210/api/tree/<GUID2>/ancestors" -H "Authorization: Bearer <token>"
