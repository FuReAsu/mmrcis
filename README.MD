# Clinical Information System

Clinical Information System built in ASP .NET Core MVC for school assignment.
The **beta** branch contains a prototype version that has up to PatientLabRecords. Abandoned it because things got convoluted real quick thanks to asking an LLM to write most of it. The main branch is mostly written by me with some views that are generated by LLM to save time. There's no decoration because I don't have much time. This is mostly to prove that I understand the flow of a webapp I guess.

## Basic Functions

- RBAC (Admin, Operator, Medical Staff)
- Staff Managemenat, AuditLogs (Administrator)
- Patient, Appointment and Check In/Out Management (Operator)
- Patient Medical Records Management (Medical Staff)
- Cost and Bills Management (didn't have enough time to implement)

## Version Information 

- MSSQL OCI image **mcr.microsoft.com/mssql/server:2022-latest**
- Entity Framework Core 9.0.6 
- .NET SDK 9.0.6

## Login Information

- Username: admin@test.local
- Password: P@ssw0rd
