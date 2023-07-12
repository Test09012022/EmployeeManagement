﻿CREATE TABLE EMPLOYEES
( EmployeeId INT IDENTITY(1,1) PRIMARY KEY ,
  FirstName VARCHAR(100) NOT NULL,
  Surname VARCHAR(100) NOT NULL,
  Email VARCHAR(100) NOT NULL,
  StartDate  DATETIME NOT NULL,
  EndDate DATETIME,
  JobTitle VARCHAR(100) NOT NULL
);

select * from employees

