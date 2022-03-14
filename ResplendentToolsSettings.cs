using System.ComponentModel;
using System.IO;
using ff14bot.Helpers;

namespace GetResplendentTools
{
    public class ResplendentToolsSettings : JsonSettings
    {
        
        private static ResplendentToolsSettings _settings;
        public static ResplendentToolsSettings Instance => _settings ?? (_settings = new ResplendentToolsSettings());
        
        public ResplendentToolsSettings() : base(Path.Combine(CharacterSettingsDirectory, "ResplendentToolsSettings.json"))
        {

        }  
        
        private bool _doCarpenter = true;
        [Description("Should we do Carpenter Resplendent Tool?")]
        [DefaultValue(true)]  
        public bool Carpenter
        
        {
            get => _doCarpenter;
            set
            {
                if (_doCarpenter != value)
                {
                    _doCarpenter = value;
                    Save();
                }
            }
        }
        
        private bool _doBlacksmith = true;
        [Description("Should we do Blacksmith Resplendent Tool?")]
        [DefaultValue(true)]  
        public bool Blacksmith
        
        {
            get => _doBlacksmith;
            set
            {
                if (_doBlacksmith != value)
                {
                    _doBlacksmith = value;
                    Save();
                }
            }
        }
        
        private bool _doArmorer = true;
        [Description("Should we do Armorer Resplendent Tool?")]
        [DefaultValue(true)]  
        public bool Armorer
        
        {
            get => _doArmorer;
            set
            {
                if (_doArmorer != value)
                {
                    _doArmorer = value;
                    Save();
                }
            }
        }
        
        private bool _doGoldsmith = true;
        [Description("Should we do Goldsmith Resplendent Tool?")]
        [DefaultValue(true)]  
        public bool Goldsmith
        
        {
            get => _doGoldsmith;
            set
            {
                if (_doGoldsmith != value)
                {
                    _doGoldsmith = value;
                    Save();
                }
            }
        }
        
        private bool _doLeatherworker = true;
        [Description("Should we do Leatherworker Resplendent Tool?")]
        [DefaultValue(true)]  
        public bool Leatherworker
        
        {
            get => _doLeatherworker;
            set
            {
                if (_doLeatherworker != value)
                {
                    _doLeatherworker = value;
                    Save();
                }
            }
        }
        
        private bool _doWeaver = true;
        [Description("Should we do Weaver Resplendent Tool?")]
        [DefaultValue(true)]  
        public bool Weaver
        
        {
            get => _doWeaver;
            set
            {
                if (_doWeaver != value)
                {
                    _doWeaver = value;
                    Save();
                }
            }
        }
        
        private bool _doAlchemist = true;
        [Description("Should we do Alchemist Resplendent Tool?")]
        [DefaultValue(true)]  
        public bool Alchemist
        
        {
            get => _doAlchemist;
            set
            {
                if (_doAlchemist != value)
                {
                    _doAlchemist = value;
                    Save();
                }
            }
        }
        
        private bool _doCulinarian = true;
        [Description("Should we do Culinarian Resplendent Tool?")]
        [DefaultValue(true)]  
        public bool Culinarian
        
        {
            get => _doCulinarian;
            set
            {
                if (_doCulinarian != value)
                {
                    _doCulinarian = value;
                    Save();
                }
            }
        }
    }
}