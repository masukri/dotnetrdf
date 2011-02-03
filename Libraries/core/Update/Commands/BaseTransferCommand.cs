﻿/*

Copyright Robert Vesse 2009-11
rvesse@vdesign-studios.com

------------------------------------------------------------------------

This file is part of dotNetRDF.

dotNetRDF is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

dotNetRDF is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with dotNetRDF.  If not, see <http://www.gnu.org/licenses/>.

------------------------------------------------------------------------

dotNetRDF may alternatively be used under the LGPL or MIT License

http://www.gnu.org/licenses/lgpl.html
http://www.opensource.org/licenses/mit-license.php

If these licenses are not suitable for your intended use please contact
us at the above stated email address to discuss alternative
terms.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VDS.RDF.Update.Commands
{
    public abstract class BaseTransferCommand : SparqlUpdateCommand
    {
        protected Uri _sourceUri, _destUri;
        protected bool _silent = false;

        public BaseTransferCommand(SparqlUpdateCommandType type, Uri sourceUri, Uri destUri, bool silent)
            : this(type, sourceUri, destUri)
        {
            this._silent = silent;
        }

        public BaseTransferCommand(SparqlUpdateCommandType type, Uri sourceUri, Uri destUri)
            : base(type, true)
        {
            this._sourceUri = sourceUri;
            this._destUri = destUri;
        }

        public Uri SourceUri
        {
            get
            {
                return this._sourceUri;
            }
        }

        public Uri DestinationUri
        {
            get
            {
                return this._destUri;
            }
        }

        public bool Silent
        {
            get
            {
                return this._silent;
            }
        }

        public override bool AffectsSingleGraph
        {
            get
            {
                return false;
            }
        }

        public override bool AffectsGraph(Uri graphUri)
        {
            if (graphUri == null || graphUri.ToString().Equals(GraphCollection.DefaultGraphUri))
            {
                return (this._destUri == null || this._sourceUri == null) || this._sourceUri.ToSafeString().Equals(GraphCollection.DefaultGraphUri) || this._destUri.ToSafeString().Equals(GraphCollection.DefaultGraphUri);
            }
            else
            {
                return graphUri.ToString().Equals(this._sourceUri.ToSafeString()) || graphUri.ToString().Equals(this._destUri.ToSafeString());
            }
            }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            switch (this.CommandType)
            {
                case SparqlUpdateCommandType.Add:
                    output.Append("ADD");
                    break;
                case SparqlUpdateCommandType.Copy:
                    output.Append("COPY");
                    break;
                case SparqlUpdateCommandType.Move:
                    output.Append("MOVE");
                    break;
                default:
                    throw new RdfException("Cannot display the String for this Transfer command as it is not one of the valid transfer commands (ADD/COPY/MOVE)");
            }

            if (this._silent) output.Append(" SILENT");

            if (this._sourceUri == null)
            {
                output.Append(" DEFAULT");
            }
            else
            {
                output.Append(" GRAPH <" + this._sourceUri.ToString().Replace(">", "\\>") + ">");
            }
            output.Append(" TO ");
            if (this._destUri == null)
            {
                output.Append(" DEFAULT");
            }
            else
            {
                output.Append(" GRAPH <" + this._destUri.ToString().Replace(">", "\\>") + ">");
            }

            return output.ToString();
        }
    }
}
