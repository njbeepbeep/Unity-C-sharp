using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeWalking {

    public class DNA {

        List<int> genes = new List<int>();
        int dnaLength = 0;
        int maxValues = 0;

        public DNA(int l, int v) {

            dnaLength = l;
            maxValues = v;
            SetRandom();
        }

        private void SetRandom() {

            genes.Clear();

            for (int i = 0; i < dnaLength; ++i) {

                genes.Add(Random.Range(0, maxValues));
            }
        }

        public void SetInt(int pos, int value) {

            genes[pos] = value;
        }

        public void Combine(DNA d1, DNA d2) {

            for (int i = 0; i < dnaLength; ++i) {

                genes[i] = i < (dnaLength / 2.0f) ? genes[i] = d1.genes[i] : genes[i] = d2.genes[i];
            }
        }

        public void Mutate() => genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
        public int GetGene(int pos) => genes[pos];
    }
}