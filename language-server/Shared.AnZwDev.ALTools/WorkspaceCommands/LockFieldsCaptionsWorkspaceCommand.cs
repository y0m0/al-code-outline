using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class LockFieldsCaptionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<LockFieldsCaptionsSyntaxRewriter>
    {

        public LockFieldsCaptionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "lockFieldsCaptions", true)
        {
        }

    }
}
