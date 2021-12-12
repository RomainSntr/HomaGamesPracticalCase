using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _sideSpeed;
    private BoxCollider _collider;
    private int _heightLevel;

    [SerializeField] private GameObject _rollingBarrel;
    private Stack<GameObject> _barrelList = new Stack<GameObject>();

    private bool _isFlying;
    private float _balloonTimer;
    [SerializeField] private float _balloonTimerLimit;

    private bool _isRunning;

    Animator _animator;

    [SerializeField] Canvas _canvas;
    [SerializeField] private UIManager UI;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _heightLevel = 0;
        _balloonTimer = 0;
        _isFlying = false;
        _isRunning = false;
        _animator = GetComponent<Animator>();
    }

    public void Play()
    {
        _isRunning = true;
        _animator.SetBool("isRunning", true);
        UI.RunningUI();
    }

    private void Die()
    {
        _isRunning = false;
        _animator.SetBool("isDead", true);
        UI.RestartUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRunning)
        {
            transform.parent.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * _forwardSpeed);

            /// Movement ///

            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                if (touch.deltaPosition.x > 0)
                {
                    this.gameObject.transform.Translate(Vector3.right * touch.deltaPosition / Screen.width * Time.deltaTime * _sideSpeed);
                }
                else if (touch.deltaPosition.x < 0)
                {
                    this.gameObject.transform.Translate(Vector3.right * touch.deltaPosition / Screen.width * Time.deltaTime * _sideSpeed);
                }

                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3, 2.5f), transform.position.y, transform.position.z);
            }

            /// Flying timer ///

            if (_isFlying)
            {
                _balloonTimer += Time.deltaTime;
                if (_balloonTimer >= _balloonTimerLimit)
                {
                    _isFlying = false;
                    _heightLevel = 0;
                    transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                    _balloonTimer = 0;
                    _animator.SetBool("isFlying", false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrel"))
        {
            _heightLevel++;
            GameObject instance = Instantiate(_rollingBarrel, this.gameObject.transform.position + new Vector3(0, 0.8f - 1.5f * _heightLevel, 0), Quaternion.identity);
            instance.transform.SetParent(this.transform);
            this.gameObject.transform.Translate(Vector3.up * 1.5f);
            _barrelList.Push(instance);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Obstacle") && (!_isFlying))
        {
            int obstacleHeight = other.GetComponent<Obstacle>().Height;
            if (obstacleHeight > _heightLevel)
            {
                Die();
            }
            else
            {
                _heightLevel -= obstacleHeight;
                for (int i = 0; i < obstacleHeight; i++)
                {
                    GameObject lastBarrel = _barrelList.Pop();
                    Destroy(lastBarrel.gameObject);
                }
                this.gameObject.transform.Translate(Vector3.down * 1.5f * obstacleHeight);
            }
            other.enabled = false;
        }
        else if (other.CompareTag("Balloon"))
        {
            _isFlying = true;
            _animator.SetBool("isFlying", true);
            foreach (GameObject barrel in _barrelList)
            {
                Destroy(barrel.gameObject);
            }
            _barrelList.Clear();
            _heightLevel = 10;
            transform.position = new Vector3(transform.position.x, 6f, transform.position.z);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Gem"))
        {
            StoreManager.Instance.GemCount++;
            UI.SetGemCount();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("FinishLine"))
        {
            _isRunning = false;
            _animator.SetBool("isRunning", false);
            UI.RestartUI();
        }

        // At the end of the loop the other collider is either disabled or has been destroyed
        // This way the same collider won't be considered in the next frame
    }
}