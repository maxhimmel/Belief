using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelateRenderPass : ScriptableRendererFeature
{
    [SerializeField] private Settings _settings = new Settings();

	private CustomRenderPass _scriptablePass;

    public override void Create()
    {
		_scriptablePass = new CustomRenderPass( _settings.Material );

		_scriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        _scriptablePass.Source = renderer.cameraColorTarget;
        renderer.EnqueuePass(_scriptablePass);
    }

    [System.Serializable]
    private class Settings
    {
        public Material Material => _material;

        [SerializeField] private Material _material = default;
    }

    private class CustomRenderPass : ScriptableRenderPass
    {
        public RenderTargetIdentifier Source;

        private Material _material;
        private RenderTargetHandle _renderTexHandle;

        public CustomRenderPass( Material material )
        {
            _material = material;
            _renderTexHandle.Init( "_TemporaryColorTexture" );
        }

        public override void OnCameraSetup( CommandBuffer cmd, ref RenderingData renderingData )
        {
        }

        public override void Execute( ScriptableRenderContext context, ref RenderingData renderingData )
        {
            CommandBuffer buffer = CommandBufferPool.Get();

            buffer.GetTemporaryRT( _renderTexHandle.id, renderingData.cameraData.cameraTargetDescriptor );
            Blit( buffer, Source, _renderTexHandle.id, _material );
            Blit( buffer, _renderTexHandle.id, Source );

            context.ExecuteCommandBuffer( buffer );
            CommandBufferPool.Release( buffer );
        }

        public override void OnCameraCleanup( CommandBuffer cmd )
        {
        }
    }
}


