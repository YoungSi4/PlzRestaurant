using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    // 테이블을 담을 배열 - 유니티 엔진 상에서 넣는 방식으로 구현.
    // ** 중요 ** 테이블 번호 순서에 맞게 넣어야 함
    [SerializeField]
    private Table[] tables = null;
    public int tableNum { get { return tables.Length; } private set {;} }


    // 지금 단계에선 더 필요한 게 없어보임.
    // 이후 필요하면 추가할 예정

    public Table GetTable(int tableNum)
    {
        if (tables == null || tables.Length <= tableNum) return null;
        return tables[tableNum - 1]; // 테이블 번호는 1번부터, 인덱스는 0번부터.
    }

    // 테이블 매니저 끄고 키는 함수
    public void EnableTableManager()
    {
        gameObject.SetActive(true);
    }

    public void DisableTableManager()
    {
        gameObject.SetActive(false);
    }
}
