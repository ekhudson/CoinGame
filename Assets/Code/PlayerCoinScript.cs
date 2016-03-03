using UnityEngine;
using System.Collections;

public class PlayerCoinScript : BaseObject 
{
    public bool FreshCoin = true;

    private const float kCurveScaleAmount = -0.05f;
    private AudioSource mAudioSource;

    public Color CoinColor = Color.grey;
    public float CoinNearAlphaAmount = 0.25f;

    private Color mNearAlphaColor = Color.grey;

    private MeshRenderer mRenderer;

    public void LaunchCoin()
    {
        PlayLaunchSound();
        SetMaterialsColor(CoinColor);
    }

    private void PlayLaunchSound()
    {
        if (mAudioSource == null)
        {
            return;
        }

        mAudioSource.Play();
    }

    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        mNearAlphaColor = CoinColor;
        mNearAlphaColor.a = CoinNearAlphaAmount;
        mRenderer = GetComponent<MeshRenderer>();

        SetMaterialsColor(mNearAlphaColor);
    }

    public void FixedUpdate()
    {
        if (CoinUserInterfaceManager.Instance.DoCurving && BaseRigidbody.angularVelocity != Vector3.zero && BaseRigidbody.velocity != Vector3.zero)
        {
            BaseRigidbody.AddForce(kCurveScaleAmount*Vector3.Cross(BaseRigidbody.velocity, BaseRigidbody.angularVelocity), ForceMode.Force);
        }
    }

    private void SetMaterialsColor(Color color)
    {
        if (mRenderer == null)
        {
            return;
        }

        foreach (Material mat in mRenderer.materials)
        {
            mat.SetColor("_Albedo", color);
        }
    }

	public void OnCollisionEnter(Collision collision)
    {
        EventManager.Instance.Post(this, new PlayerCoinImpactEvent(this, this, collision));
    }
}
