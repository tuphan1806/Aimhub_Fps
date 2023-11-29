using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GunManager : MonoBehaviour
{
    //Gun stats
    [Header("Gun stats")]
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int megazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public float shootForce, upwardForce;
    //Recoil
    public float rotationSpeed = 6;
    public float returnSpeed = 25;
    public Vector3 recoilRotation = new Vector3 (2f, 2f, 2f);

    private Vector3 currentRotation;
    private Vector3 rot;

    //bullets
    int bulletsLeft, bulletsShot;

    // bools
    bool shooting, readyToShoot, reloading;

    //score
    public int point = 5;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public LayerMask whatIsEnemy;
    private GameManager gameManager;

    //Sfx
    public AudioSource gunSound;

    //Graphics
    public GameObject muzzleFlash;// bulletHoldGraphic;
    public GameObject bloodSpash;
    
    //public TextMeshProUGUI text;

    public GameObject bullet;
    private void Awake()
    {
        bulletsLeft = megazineSize;
        readyToShoot = true;
        reloading = false;
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();      
    }

    private void FixedUpdate()
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        rot = Vector3.Slerp(rot, currentRotation, rotationSpeed * Time.deltaTime);
        fpsCam.transform.localRotation = Quaternion.Euler(rot);
    }

    private void Update()
    {
        if (gameManager.isGameActive)
        {
            MyInput();
        }
        //SetText
        //text.SetText(bulletsLeft +  " / " + megazineSize);
    }

    private void MyInput()
    {
        
            if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
            else shooting = Input.GetKeyDown(KeyCode.Mouse0);

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < megazineSize && !reloading) Reload();

            //Shoot
            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
            {
                bulletsShot = bulletsPerTap;
                Shoot();
                gunSound.Play();
            }  
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
        RaycastHit hit;
        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
            if (hit.collider.tag == "Target")
            {
                GameObject target = hit.collider.gameObject;
                GameObject blood = Instantiate(bloodSpash, targetPoint, Quaternion.identity);
                Destroy(blood, 1f);
                Destroy(target);
                gameManager.UpdateScore(point);
            }
            if (hit.collider.tag == "Head")
            {   
                GameObject target = hit.collider.gameObject;
                GameObject blood = Instantiate(bloodSpash, targetPoint, Quaternion.identity);       
                Destroy(target.transform.parent.gameObject);
                Destroy(blood, 1f);
                gameManager.UpdateScore(point + 2);
            }
            if (hit.collider.tag == "Body")
            {
                GameObject target = hit.collider.gameObject;
                GameObject blood = Instantiate(bloodSpash, targetPoint, Quaternion.identity);
                Destroy(target.transform.parent.gameObject);
                Destroy(blood, 1f);
                gameManager.UpdateScore(point - 2);
            }
        }
            
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, transform.rotation);
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Graphics
        GameObject Flash = Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);
        Destroy(Flash, 0.1f);
        //Instantiate(bulletHoldGraphic, hit.point, Quaternion.Euler(0, 180, 0));
        
        
        

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

        //Recoil
        currentRotation += new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = megazineSize;
        reloading = false;
    }

}
