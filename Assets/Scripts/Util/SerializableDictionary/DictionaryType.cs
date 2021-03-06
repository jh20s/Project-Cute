using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SkillDic : SerializableDictionary<Skill, List<Projectile>> { }
[System.Serializable]
public class IntGameObject : SerializableDictionary<int, GameObject> { }
[System.Serializable]
public class StringGameObject : SerializableDictionary<string, GameObject> { }
[System.Serializable]
public class IntStringGameObject : SerializableDictionary<int, StringGameObject> { }
[System.Serializable]
public class StringIntGameObject : SerializableDictionary<string, IntGameObject> { }
[System.Serializable]
public class TypeSprite : SerializableDictionary<EquipmentManager.SpriteType, EquipmentManager.CostumeSprite> { }
[System.Serializable]
public class CstBuffTypeValue : SerializableDictionary<Costume.CostumeBuffType, int> { }

[System.Serializable]
public class MonsterDataSet : SerializableDictionary<Tuple<string,int>, MonsterManager.MonsterData> { }

[System.Serializable]
public class TrainingSet : SerializableDictionary<TrainingManager.TrainingType, TrainingManager.Training> { }

[System.Serializable]
public class TrainingButtonSet : SerializableDictionary<TrainingManager.TrainingType, GameObject> { }

[System.Serializable]
public class StringBoolean : SerializableDictionary<string, bool> { }

[System.Serializable]
public class StringInt : SerializableDictionary<string, int> { }

[System.Serializable]
public class StringIntInt : SerializableDictionary<string, SerializableDictionary<int, int>> { }

[System.Serializable]
public class StringAchievement : SerializableDictionary<string, Achievement> { }

[System.Serializable]
public class StringState : SerializableDictionary<string, Achievement.AState> { }

[System.Serializable]
public class IntInt : SerializableDictionary<int, int> { }

[System.Serializable]
public class UIAudoiData : SerializableDictionary<LobbyMusicManager.AudioType, AudioClip> { }

