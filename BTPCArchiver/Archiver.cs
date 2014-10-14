namespace BTPCArchiver
{
    using System;
    using System.IO;
    using System.Text;
    using System.Drawing;
    using System.Resources;
    using System.Reflection;
    using System.Diagnostics;
    using System.Collections;
    using System.ComponentModel;
    using Microsoft.BizTalk.Message.Interop;
    using Microsoft.BizTalk.Component.Interop;
    using Microsoft.BizTalk.Component;
    using Microsoft.BizTalk.Messaging;
    
    
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("c691e815-de36-4792-8015-50d2519ff346")]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    public class Archiver : Microsoft.BizTalk.Component.Interop.IComponent, IBaseComponent, IPersistPropertyBag, IComponentUI
    {
        
        private System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("BTPCArchiver.Archiver", Assembly.GetExecutingAssembly());
        
        private string _PropertyName;
        
        public string PropertyName
        {
            get
            {
                return _PropertyName;
            }
            set
            {
                _PropertyName = value;
            }
        }
        
        private bool _Enabled;
        
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                _Enabled = value;
            }
        }
        
        private string _FilenameExtension;
        
        public string FilenameExtension
        {
            get
            {
                return _FilenameExtension;
            }
            set
            {
                _FilenameExtension = value;
            }
        }
        
        private string _PropertyNS;
        
        public string PropertyNS
        {
            get
            {
                return _PropertyNS;
            }
            set
            {
                _PropertyNS = value;
            }
        }
        
        private string _Filepath;
        
        public string Filepath
        {
            get
            {
                return _Filepath;
            }
            set
            {
                _Filepath = value;
            }
        }

        private Guid _MessageID;

        public Guid MessageID
        {
            get
            {
                return _MessageID;
            }
            set
            {
                _MessageID = value;
            }
        }
        
        #region IBaseComponent members
        /// <summary>
        /// Name of the component
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get
            {
                return resourceManager.GetString("COMPONENTNAME", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        
        /// <summary>
        /// Version of the component
        /// </summary>
        [Browsable(false)]
        public string Version
        {
            get
            {
                return resourceManager.GetString("COMPONENTVERSION", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        
        /// <summary>
        /// Description of the component
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get
            {
                return resourceManager.GetString("COMPONENTDESCRIPTION", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        #endregion
        
        #region IPersistPropertyBag members
        /// <summary>
        /// Gets class ID of component for usage from unmanaged code.
        /// </summary>
        /// <param name="classid">
        /// Class ID of the component
        /// </param>
        public void GetClassID(out System.Guid classid)
        {
            classid = new System.Guid("c691e815-de36-4792-8015-50d2519ff346");
        }
        
        /// <summary>
        /// not implemented
        /// </summary>
        public void InitNew()
        {
        }
        
        /// <summary>
        /// Loads configuration properties for the component
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="errlog">Error status</param>
        public virtual void Load(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, int errlog)
        {
            object val = null;
            val = this.ReadPropertyBag(pb, "PropertyName");
            if ((val != null))
            {
                this._PropertyName = ((string)(val));
            }
            val = this.ReadPropertyBag(pb, "Enabled");
            if ((val != null))
            {
                this._Enabled = ((bool)(val));
            }
            val = this.ReadPropertyBag(pb, "FilenameExtension");
            if ((val != null))
            {
                this._FilenameExtension = ((string)(val));
            }
            val = this.ReadPropertyBag(pb, "PropertyNS");
            if ((val != null))
            {
                this._PropertyNS = ((string)(val));
            }
            val = this.ReadPropertyBag(pb, "Filepath");
            if ((val != null))
            {
                this._Filepath = ((string)(val));
            }
        }
        
        /// <summary>
        /// Saves the current component configuration into the property bag
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="fClearDirty">not used</param>
        /// <param name="fSaveAllProperties">not used</param>
        public virtual void Save(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, bool fClearDirty, bool fSaveAllProperties)
        {
            this.WritePropertyBag(pb, "PropertyName", this.PropertyName);
            this.WritePropertyBag(pb, "Enabled", this.Enabled);
            this.WritePropertyBag(pb, "FilenameExtension", this.FilenameExtension);
            this.WritePropertyBag(pb, "PropertyNS", this.PropertyNS);
            this.WritePropertyBag(pb, "Filepath", this.Filepath);
        }
        
        #region utility functionality
        /// <summary>
        /// Reads property value from property bag
        /// </summary>
        /// <param name="pb">Property bag</param>
        /// <param name="propName">Name of property</param>
        /// <returns>Value of the property</returns>
        private object ReadPropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName)
        {
            object val = null;
            try
            {
                pb.Read(propName, out val, 0);
            }
            catch (System.ArgumentException )
            {
                return val;
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException(e.Message);
            }
            return val;
        }
        
        /// <summary>
        /// Writes property values into a property bag.
        /// </summary>
        /// <param name="pb">Property bag.</param>
        /// <param name="propName">Name of property.</param>
        /// <param name="val">Value of property.</param>
        private void WritePropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName, object val)
        {
            try
            {
                pb.Write(propName, ref val);
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException(e.Message);
            }
        }
        #endregion
        #endregion
        
        #region IComponentUI members
        /// <summary>
        /// Component icon to use in BizTalk Editor
        /// </summary>
        [Browsable(false)]
        public IntPtr Icon
        {
            get
            {
                return ((System.Drawing.Bitmap)(this.resourceManager.GetObject("COMPONENTICON", System.Globalization.CultureInfo.InvariantCulture))).GetHicon();
            }
        }
        
        /// <summary>
        /// The Validate method is called by the BizTalk Editor during the build 
        /// of a BizTalk project.
        /// </summary>
        /// <param name="obj">An Object containing the configuration properties.</param>
        /// <returns>The IEnumerator enables the caller to enumerate through a collection of strings containing error messages. These error messages appear as compiler error messages. To report successful property validation, the method should return an empty enumerator.</returns>
        public System.Collections.IEnumerator Validate(object obj)
        {
            // example implementation:
            // ArrayList errorList = new ArrayList();
            // errorList.Add("This is a compiler error");
            // return errorList.GetEnumerator();
            return null;
        }
        #endregion
        
        #region IComponent members
        public Microsoft.BizTalk.Message.Interop.IBaseMessage Execute(Microsoft.BizTalk.Component.Interop.IPipelineContext pc, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg)
        {
            if (null == pc)
                throw new ArgumentNullException("PC is null");
            if (null == inmsg)
                throw new ArgumentNullException("inmsg is null");

            MessageID = inmsg.MessageID;
            ArchiverStream archiverStream = new ArchiverStream(inmsg.BodyPart.Data, this);
            pc.ResourceTracker.AddResource(archiverStream);
            inmsg.BodyPart.Data = archiverStream;
            try
            {
                inmsg.Context.Promote(_PropertyName, _PropertyNS, archiverStream.finalOutputFilepath());
            }
            catch (Exception e)
            { }
            return inmsg;
        }
        #endregion
    }
}
