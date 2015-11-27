# controlpanel
Windows Service Tracking and Dynamic task distribution

Steps to integrate control panel are as follows:

Step 1: add http handlers in web.config
 <handlers>
      <add path="performance.axd" verb="*" type="ControlPanel.Core.PerformanceStatisticHandler" name="performance"/>
      <add path="controlpanel.axd" verb="*" type="ControlPanel.Core.ControlPanelHandler" name="controlpanel"/>
 </handlers>
  
  Step 2: add connection string in web.config
  <add name="controlpanel" connectionString="Initial Catalog=ControlPanel;Data Source=<database>;User Id=<user>; Password=<password>"/>
  
  
