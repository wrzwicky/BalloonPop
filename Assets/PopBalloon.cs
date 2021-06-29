using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Prefab Notes:
// * Polygon Collider for click precision
// * Circle Collider cuz poly doesn't resolve overlaps when first instantiated

//TODO remove bass from bump sounds; make more squeaky and less impacty
//TODO ensure fingerpoke is registering good location; balloons keep refusing to pop on phone

/// <summary>
/// Code for the poppable balloon.
/// </summary>
public class PopBalloon : MonoBehaviour
{
    public Sprite popImage;
    public Texture popTexture;

    [Tooltip("List of sounds available to play when balloon pops.")]
    public AudioClip[] popSounds;
    [Tooltip("Loudness of popSounds.")]
    public float popGain = 1f;
    [Tooltip("List of sounds available to play when balloons bump.")]
    public AudioClip[] bumpSounds;
    [Tooltip("Loudness of bumpSounds.")]
    public float bumpGain = 1f;
    [Tooltip("Health of new balloon.")]
    public int startHP = 2;

    private Collider m_Collider;
    private Rigidbody2D m_RB;
    private Rigidbody2D m_RB_tail;
    private SpriteRenderer m_SR;
    private ScoreKeeper m_scoreKeeper;
    private AudioSource m_audio;

    // current health
    private int hp;
    // false = use float physics; true = use tumble physics
    private bool popped = false;

    // Start is called before the first frame update
    void Start()
    {
        hp = Random.Range(1, startHP+1);

        m_Collider = GetComponent<Collider>();
        m_RB = GetComponent<Rigidbody2D>();
        m_RB_tail = GetComponentInChildren<Rigidbody2D>();
        m_SR = GetComponent<SpriteRenderer>();
        m_scoreKeeper = FindObjectOfType<ScoreKeeper>();
        m_audio = GetComponent<AudioSource>();

        m_SR.color = Random.ColorHSV(0f,1f, 1f,1f, 1f,1f);
        // See material/3dots/Edit Shader to find name of tint param
        m_SR.material.SetColor("_BaseColor", m_SR.color);

        m_RB.gravityScale += Random.Range(-0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        // Right side up
        if (!popped)
        {
            float targetAngle = Vector3.SignedAngle(Vector3.down, Physics2D.gravity, Vector3.forward);
            float av = m_RB.angularVelocity;
            Mathf.SmoothDampAngle(m_RB.rotation, targetAngle, ref av, 0.25f);
            m_RB.angularVelocity = av;
        }
    }

    private void OnMouseDown()
    {
        Pop(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    Destroy(gameObject);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int r = Random.Range(0, bumpSounds.Length);
        m_audio.pitch = Random.Range(0.9f, 1.1f);
        m_audio.PlayOneShot(bumpSounds[r], bumpGain);
    }

    private void Pop(Vector3 pokePos)
    {
        m_RB.AddForce((transform.position - pokePos) * 30f, ForceMode2D.Impulse);

        if (--hp > 0)
        {
            int rb = Random.Range(0, bumpSounds.Length);
            m_audio.pitch = Random.Range(0.9f, 1.1f);
            m_audio.PlayOneShot(bumpSounds[rb], bumpGain);

            return;
        }

        popped = true;
        m_scoreKeeper.Score += 1;

        int rp = Random.Range(0, popSounds.Length);
        m_audio.pitch = Random.Range(0.9f, 1.1f);
        m_audio.PlayOneShot(popSounds[rp], popGain);

        m_SR.sprite = popImage;
        m_SR.material.SetTexture("_BaseMap", popImage.texture);

        //transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        m_RB.rotation = Random.Range(0, 360);
        m_RB.angularVelocity = (Random.value >= 0.5 ? -1 : 1) * Random.Range(300f, 1200f);
        m_RB.gravityScale = 5;
        Destroy(gameObject, 0.2f);
    }

}
