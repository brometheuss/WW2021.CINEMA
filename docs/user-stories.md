#User Stories:


##Must Do Functionalities:

|**User Story**|**Description**|
|:----------------:|:----------------:|    
|**Ticket Reservation**|Client (register user - confirmed user) should be able to reserve ticket by clicking on desired seats. When we select desired seat we proceed to payment. If payment is successful client have ticket and seat reserved. Application must display already reserved seats and mark them with difrente color from the available ones.<br/> If client reserves more than one seat, seats must be next to each other.<br/> Corner Case: If we want to reserve two or more eats and we do not have them available in row one after another. Application should inform us that we can buy tickets separately or change projection time of specific movie. Develop backend and frontend for this functionality.|
|**Activate/ Deactivate Movie**|Client (administrator user – confirmed admin user) should be able to activate or deactivate movie(current field).<br/>Develop backend and frontend for this functionality. If movie have projections which are in future it can not be deactivated.|
|**Movie Search**|Client (confirmed users) should be able to search for specific movie by filtering tags. Tags can be unlimited, like (Actor, Year, Title….)<br/>Develop backend and frontend for this functionality.|
|**Filter Projections**|Client (confirmed users) should be able to filter by Cinema, Auditorium, Movie, Specific Time Span. Projections are searchable both separately or it can be based on multiple parameters. You should not be able to pass Auditorium which is not part of selected Cinema.<br/>Develop backend and frontend for this functionality.|
|**Unit Tests**| Cover controllers and services with unit tests.|
|**Movie Top List**|Client (confirmed users) should be able to see top 10 movie according to ratings.<br/>Develop backend and frontend for this functionality.|
|**Create Cinema**|Client (administrator user – confirmed admin user) should be able to create new cinema.<br/>Develop backend and frontend for this functionality.|
|**Presenting Application**|Demo for application developed functionalities. Present which functionalities are developed.|
|**Build – Release Pipeline**|Investigate and present in your presentation this topic.|

##Bonus functionalities:

|**User Story**|**Description**|
|:----------------:|:----------------:|
|**Cinema Bonus Points** - (connected to must do task 1.)| Client (registered user - confirmed user) should be able to collect bonus points for successful ticket purchase. For every successful ticket purchase, one bonus point is added for current user (registered user - confirmed user). All collected points should be visible on application "Profile page".<br/>Develop backend and frontend for this functionality.|
|**Movies Top List** - (connected to must do task 6.)|client (registered user - confirmed user) should be able to see top list for specific year. If there are more movies with same rating we can rate them further if some of them have Oscar.<br/>Develop backend and frontend for this functionality.|
|**Create Cinema** - (connected to must do task 7.)|Client (administrator user – confirmed admin user) should be able to create through creation of cinema also auditorium with seats.<br/>Develop backend and frontend for this functionality.|
|**Delete Cinema**|Client (administrator user – confirmed admin user) should be able to delete cinema with all its auditoriums and seats.<br/>Develop backend and frontend for this functionality.|
|**Introduce New Role**|Client should be able to log as different type of users. For example: admin, super_user, user.<br/>Develop backend and frontend for this functionality.|
|**Refactoring Client App**|Try to move, post, get, put and delete to single file and export them as functions. <br/>Develop this functionality in Client app.|
|**Add Route For New User Roles**|Add route protection for new user role.<br/>Develop backend and frontend for this functionality.|
|**Performance Improvment**|Change some complicated queries with views. Present all improvments.|
|**Add 3rd party API integration**|Integrate application with 3rd party api(IMDb api). Enrich movie data with IMDb user rating and some picture(s). Feel free to add anything you want to improve user experience.|
|**Add 3rd party API integration**|Integrate application with 3rd party api(IMDb api). Get Top 10 most popular movies from IMDb.|
