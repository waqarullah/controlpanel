# controlpanel
Windows Service Tracking and Dynamic task distribution

  Steps to configure control panel
- Add ControlPanel.Core to the solution
- Update spring.core package
- Target framework should match the windows service target framework
- Web.Config changes
- Add controlpanel database connection string
  <add name="controlpanel" connectionString="Initial Catalog=ControlPanel;Data Source=<database>;User Id=<user>; Password=<password>"/>
- Add httphandler for ControlPanel.axd
  <handlers>
      <add path="performance.axd" verb="*" type="ControlPanel.Core.PerformanceStatisticHandler" name="performance"/>
      <add path="controlpanel.axd" verb="*" type="ControlPanel.Core.ControlPanelHandler" name="controlpanel"/>
 </handlers>
- Host the project and run /controlpanel.axd, it will create the database
- Insert the information in SystemProcess table
- Create windows service project
- In service1.cs file, inherit the service1 class from ControlPanel.ServiceBaseExtention
- Remove OnStart and OnStop events from service1 class
- Create ProcessThreads class library
- Create threads
- Enter thread information in systemprocessthread table
