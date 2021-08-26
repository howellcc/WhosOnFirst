# Whos On First
Simple app for randoming the names of my teammates for our morning standup. I found over a teams call that if I always called on everyone in the same order, my teammates weren't paying attention to each other. I remedied this by writing this app to randomize the order and keep them on their toes. 

## How To Configure
In the App.config you'll find the following
```
<appSettings>
   <!-- comma separated list of team members -->
   <add key="TeamMembersList" value="Team Member 1, Team Member 2, Team Member 3" />
      
   <!-- Managers name will come last -->
   <add key="ManagerName" value="Team Lead"/>
</appSettings>
```
Simply populate the TeamMembersList with a comma delimited list of the members of your team, and your name in the ManagerName field. Then compile and run. 
