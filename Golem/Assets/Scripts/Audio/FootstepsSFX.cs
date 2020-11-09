﻿using UnityEngine;

public class FootstepsSFX : MonoBehaviour
{
    private enum FOOT { LEFT, RIGHT };

    [SerializeField] private float _threshold;

    [CustomHeader("Bones")]
    [SerializeField] private Transform _leftFootParentBone;
    [SerializeField] private Transform _rightFootParentBone;
    [SerializeField] private Transform _leftFoot;
    [SerializeField] private Transform _rightFoot;

    [CustomHeader("Audio")]
    [SerializeField] private OneShotEmitter _sfxEmitter;

    [CustomHeader("VFX")]
    [SerializeField] private ParticleSystem _leftFootVFX;
    [SerializeField] private ParticleSystem _rightFootVFX;

    private Transform[] _parentBones;
    private Transform[] _feet;
    private float[] _feetPos = new float[2];
    private bool[] _feetAboveThreshold = new bool[2];

    private void Awake()
    {
        _feet = new Transform[2];
        _feet[0] = _leftFoot;
        _feet[1] = _rightFoot;

        _parentBones = new Transform[2];
        _parentBones[0] = _leftFootParentBone;
        _parentBones[1] = _rightFootParentBone;
    }

    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            _feetPos[i] = _feet[i].position.y;

            if (_feetPos[i] - _parentBones[i].position.y > _threshold)
                _feetAboveThreshold[i] = true;

            if (_feetPos[i] - _parentBones[i].position.y <= _threshold && _feetAboveThreshold[i])
                Step((FOOT)i);
        }
    }

    int i = 0;
    private void Step(FOOT foot)
    {
        i++;

        if (i > 1)
        {
            if (foot == FOOT.LEFT)
                _leftFootVFX.Play();
            else
                _rightFootVFX.Play();

            DebugWrapper.Log("Step");
            _sfxEmitter.Play();
            _feetAboveThreshold[(int)foot] = false;
        }
    }
}
