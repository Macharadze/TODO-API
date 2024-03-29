# To-Do API

This web API project is designed to manage To-Do lists with users, to-dos, and subtasks. Below are the technical requirements, data structures, and acceptance criteria for this application.

## Technical Infrastructure Requirements

- Web API built on .NET 6
- Layered architecture (Onion or Clean Architecture)
- Middleware for global exception handling and request-response logging

## Data Structures

### User
- Username 
- PasswordHash

### To-Do
- Title 
- Status 
- Target completion date 
- List of subtasks
- OwnerI

### Subtask
- Title (Max Length: 100)
- TodoItemId

### ActionLogs 
- Date
- ItemType
- ItemId
- OperationType (Enum: Created, Updated, Deleted, MarkedAsDone, ...)
- ColumnName
- OldResult
- NewResult

### BaseEntity / IEntity
- Id
- CreatedAt
- ModifiedAt
- Status (Active, Deleted)

## Acceptance Criteria

- Users can register to use the to-do application by making a POST request to `/v1/users` with username and password in the body.
- Users can login to the API by making a POST request to `/v1/users/access-token` with username and password. If authenticated, a JWT is returned.
- Authenticated users can create a new To-Do item by making a POST request to `/v1/todos/` with Title and target completion date in the body.
- Authenticated users can retrieve all their to-dos by making a GET request to `/v1/todos/`. Optionally, they can filter by to-do status.
- Authenticated users can retrieve a specific to-do by making a GET request to `/v1/todos/{{id}}`. If the to-do does not exist or is deleted, a 404 error is returned.
- Authenticated users can edit an existing to-do item by making PUT or PATCH requests to `/v1/todos/{{id}}` with title and target completion date.
- Authenticated users can update the status of a to-do item by making a POST request to `/v1/todos/{{id}}/done`.
- Authenticated users can delete a to-do by making a DELETE request to `/v1/todos/{{id}}`.
- Authenticated users can add a subtask by making a POST request to `/v1/subtasks/` with title and todoItemId in the body.
- Users can perform CRUD operations on subtasks.
- Actions made on to-do items are logged. Users can retrieve the action list for a specific to-do item or subtask.