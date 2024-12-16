-- Create Courses table
CREATE TABLE Courses (
    courses_ID INT PRIMARY KEY IDENTITY(1,1),
    courses_Name NVARCHAR(255) NOT NULL,
    courses_Credit INT NOT NULL
);

-- Create Semesters table
CREATE TABLE Semesters (
    semesters_ID INT PRIMARY KEY IDENTITY(1,1),
    semesters_Name NVARCHAR(255) NOT NULL
);

-- Create Departments table
CREATE TABLE Departments (
    departments_ID INT PRIMARY KEY IDENTITY(1,1),
    departments_Name NVARCHAR(255) NOT NULL
);

-- Create Days table
CREATE TABLE Days (
    days_ID INT PRIMARY KEY IDENTITY(1,1),
    days_Name NVARCHAR(255) NOT NULL
);

-- Create Years table
CREATE TABLE Years (
    years_ID INT PRIMARY KEY IDENTITY(1,1),
    years_Name NVARCHAR(255) NOT NULL
);

-- Create Rooms table
CREATE TABLE Rooms (
    rooms_ID INT PRIMARY KEY IDENTITY(1,1),
    room_Name NVARCHAR(255) NOT NULL,
    room_Capacity INT
);

-- Create Periods table
CREATE TABLE Periods (
    periods_ID INT PRIMARY KEY IDENTITY(1,1),
    periods_Name NVARCHAR(255) NOT NULL
);

-- Create Professors table
CREATE TABLE Professors (
    professors_ID INT PRIMARY KEY,
    professors_Name NVARCHAR(255) NOT NULL,
    professors_PhoneNumber NVARCHAR(255),
    professors_UserName NVARCHAR(255),
    professors_Password NVARCHAR(255)
);

-- Create CoursesDetails table
CREATE TABLE CoursesDetails (
    coursesdetails_ID INT PRIMARY KEY,
    coursesdetails_courses_ID INT,
    coursesdetails_semesters_ID INT,
    coursesdetails_departments_ID INT,
    CoursesDetails_years_ID INT,
    CoursesDetails_days_ID INT,
    CoursesDetails_rooms_ID INT,
    coursesdetails_periods_ID INT,
    coursesdetails_IsEnd BIT,
    coursesdetails_professors_ID INT,
    FOREIGN KEY (coursesdetails_courses_ID) REFERENCES Courses(courses_ID),
    FOREIGN KEY (coursesdetails_semesters_ID) REFERENCES Semesters(semesters_ID),
    FOREIGN KEY (coursesdetails_departments_ID) REFERENCES Departments(departments_ID),
    FOREIGN KEY (CoursesDetails_years_ID) REFERENCES Years(years_ID),
    FOREIGN KEY (CoursesDetails_days_ID) REFERENCES Days(days_ID),
    FOREIGN KEY (CoursesDetails_rooms_ID) REFERENCES Rooms(rooms_ID),
    FOREIGN KEY (coursesdetails_periods_ID) REFERENCES Periods(periods_ID),
    FOREIGN KEY (coursesdetails_professors_ID) REFERENCES Professors(professors_ID),
    CONSTRAINT Unique_Course_Scheduling UNIQUE (coursesdetails_years_ID, coursesdetails_days_ID, CoursesDetails_rooms_ID, coursesdetails_periods_ID)
);

-- Create Students table
CREATE TABLE Students (
    students_ID INT PRIMARY KEY,
    students_Name NVARCHAR(255) NOT NULL,
    students_PhoneNumber VARCHAR(15),
    students_BirthDay DATE,
    students_GPA DECIMAL(3, 2),
    students_departments_ID INT,
    students_SemesterIN INT,
    students_TotalCreidts INT,
    students_Username NVARCHAR(255),
    students_Password NVARCHAR(255),
    FOREIGN KEY (students_departments_ID) REFERENCES Departments(departments_ID),
    FOREIGN KEY (students_SemesterIN) REFERENCES Semesters(semesters_ID)
);

-- Create StudentCourses table
CREATE TABLE StudentCourses (
    students_ID INT,
    coursesdetails_ID INT,
    FOREIGN KEY (students_ID) REFERENCES Students(students_ID),
    FOREIGN KEY (coursesdetails_ID) REFERENCES CoursesDetails(coursesdetails_ID),
    CONSTRAINT Unique_Student_Course UNIQUE (students_ID, coursesdetails_ID)
);

-- Create CoursesExamsDetails table
CREATE TABLE CoursesExamsDetails (
    coursesexamsdetails_ID INT PRIMARY KEY IDENTITY(1,1),
    coursesexamsmarks_coursesexamsdetails_ID INT,
    coursesexamsdetails_Name NVARCHAR(255) NOT NULL,
    coursesexamsdetails_Date DATE,
    coursesexamsdetails_TotalMarks INT,
    FOREIGN KEY (coursesexamsmarks_coursesexamsdetails_ID) REFERENCES CoursesDetails(coursesdetails_ID)
);

-- Create CoursesExamsMarks table
CREATE TABLE CoursesExamsMarks (
    coursesexamsmarks_ID INT PRIMARY KEY IDENTITY(1,1),
    coursesexamsmarks_coursesexamsdetails_ID INT,
    coursesexamsdetails_students_ID INT,
    coursesexamsdetails_MarkOfStudent DECIMAL(5, 2),
    FOREIGN KEY (coursesexamsmarks_coursesexamsdetails_ID) REFERENCES CoursesExamsDetails(coursesexamsdetails_ID),
    FOREIGN KEY (coursesexamsdetails_students_ID) REFERENCES Students(students_ID)
);

-- Create AdminUsers table
CREATE TABLE AdminUsers (
    adminusers_ID INT PRIMARY KEY IDENTITY(1,1),
    adminusers_UserName NVARCHAR(255),
    adminusers_Password NVARCHAR(255)
);