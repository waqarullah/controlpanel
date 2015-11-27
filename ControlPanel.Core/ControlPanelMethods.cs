using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ControlPanel.Core
{
    public class ControlPanelMethods
    {
       
        public void WriteFile(string binPath, string springfilepath)
        {
            if (!File.Exists(springfilepath))
            {

                StringBuilder text = new StringBuilder();

                text.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                text.Append("<objects xmlns=\"http://www.springframework.net\">");
                text.Append("<object name=\"CPWebClient\" type=\"ControlPanel.Core.CPServiceClient , ControlPanel.Core\"></object>");
                text.Append("<object name=\"SystemProcessRepository\" type=\"ControlPanel.Repository.SystemProcessRepository , ControlPanel.Repository\"></object>");
                text.Append("<object name=\"SystemProcessService\" type=\"ControlPanel.Service.SystemProcessService , ControlPanel.Service\">");
                text.Append("<constructor-arg name=\"iSystemProcessRepository\" ref=\"SystemProcessRepository\"/>");
                text.Append("</object>");
                text.Append("<object name=\"SystemProcessThreadRepository\" type=\"ControlPanel.Repository.SystemProcessThreadRepository , ControlPanel.Repository\"></object>");
                text.Append("<object name=\"SystemProcessThreadService\" type=\"ControlPanel.Service.SystemProcessThreadService , ControlPanel.Service\">");
                text.Append("<constructor-arg name=\"iSystemProcessThreadRepository\" ref=\"SystemProcessThreadRepository\"/>");
                text.Append("</object>");
                text.Append("<object name=\"SystemEventLogRepository\" type=\"ControlPanel.Repository.SystemEventLogRepository , ControlPanel.Repository\"></object>");
                text.Append("<object name=\"SystemEventLogService\" type=\"ControlPanel.Service.SystemEventLogService , ControlPanel.Service\">");
                text.Append("<constructor-arg name=\"iSystemEventLogRepository\" ref=\"SystemEventLogRepository\"/>");
                text.Append("</object>");
                text.Append("</objects>");

                File.WriteAllText(binPath + "\\" + "ControlPanelSpring.cfg.xml", text.ToString());
            }
        }

    }
}