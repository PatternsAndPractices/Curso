using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace XmlData
{
    /// <summary>
    /// Descripcion del objetivo de la clase
    /// </summary>
    public class MessageHelper
    {
        // Constantes
        #region Contantes
        private const string MESSAGEFILE = @"D:\Tmp\Samples\Patterns\Patterns\Files\Messages.xml";
        #endregion Contantes

        // Campos o Atributos
        #region Campos o Atributos
        //private static MessageHelper instance = new MessageHelper();
        private static MessageHelper instance;
        private IList<MessageData> items;
        // Lock synchronization object
        private static object syncLock = new object();
        #endregion Campos o Atributos

        // Constructores
        #region Constructores
        private MessageHelper()
        {
            this.items = this.LoadData(MESSAGEFILE);
        }

        public IList<MessageData> Items { get => items; }
        #endregion Constructores

        // Metodos
        #region Metodos

        // Publicos
        #region Publicos
        public static MessageHelper GetInstance()
        {
            lock (syncLock)
            {
                if (instance == null)
                {
                    instance = new MessageHelper();
                }
            }
            return instance;
        }

        #endregion Publicos

        // Privados
        #region Privados
        private IList<MessageData> LoadData(string messageFile)
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(Messages));
            StreamReader srdr = new StreamReader(messageFile);
            Messages data = (Messages)xmlser.Deserialize(srdr);
            srdr.Close();

            return data.Items;
        }
        #endregion Privados

        #endregion Metodos
    }
}
