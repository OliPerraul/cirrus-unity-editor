using System; using Cirrus.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Cirrus; using Cirrus.Objects;

namespace Cirrus.UnityEditorExt
{
    public class LayerHelperComponent :  MonoBehaviour
    {
        [Serializable]
        public enum Mode
        {
            StartsWith,
            Match,
            Regex// TODO
        }

        [SerializeField]
        private bool _inverseMatchCondition = false;

        [SerializeField]
        private Mode _mode = Mode.StartsWith;

        [SerializeField]
        private string _search = "";

        [SerializeField]
        private bool _recursivelyChangeChildren;

        [SerializeField]        
        public bool _removeFromLayer = false;

        [SerializeField]
        private LayerMask _targetLayer;

        public void DoRecurseUpdateLayers( Transform trans )
        {
            if ( !_removeFromLayer )
            {               
                int val = _targetLayer.value == 0 ? 0 : BitwiseUtils.Position( _targetLayer.value ) - 1;

                //trans.gameObject.layer |= _layerMask;
                trans.gameObject.layer = val;
            }
            else
            {
                trans.gameObject.layer = 0;
            }

            if ( _recursivelyChangeChildren )
            {
                foreach ( Transform child in trans )
                {
                    if ( child == null ) continue;
                    DoRecurseUpdateLayers( child );
                }
            }
        }

        public void RecurseUpdateLayers(Transform trans)
        {
            foreach( Transform child in trans )
            {
                if ( child == null ) continue;

                switch ( _mode )
                {
                    case Mode.StartsWith:

                        bool match = child.gameObject.name.StartsWith( _search );

                        match = _inverseMatchCondition ? !match : match;

                        if ( match )
                        {
                            DoRecurseUpdateLayers( child );
                        }

                        break;
                }

                RecurseUpdateLayers( child );
            }
        }

        public void UpdateLayers()
        {
            RecurseUpdateLayers( transform );
        }
    }


#if UNITY_EDITOR
    [CustomEditor( typeof( LayerHelperComponent ) )]
    public class LayerHelperComponentEditor : Editor
    {
        private LayerHelperComponent _chunk;

        private bool _areMapsEnabled = false;

        public void OnEnable()
        {
            _chunk = target as LayerHelperComponent;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if ( GUILayout.Button( "Update Layers" ) )
            {
                _chunk.UpdateLayers();
            }
        }
    }
#endif


}
